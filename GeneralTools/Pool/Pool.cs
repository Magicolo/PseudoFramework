using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using System.Threading;
using Pseudo.Internal;
using System;
using Pseudo.Internal.Pool;

namespace Pseudo
{
	public class Pool<T> : Pool where T : class
	{
		public Pool(T reference, int startSize) : base(reference, reference.GetType(), null, null, startSize, true) { }

		public Pool(T reference, Constructor<T> constructor, int startSize) : base(reference, reference.GetType(), constructor, null, startSize, true) { }

		public Pool(T reference, Constructor<T> constructor, Destructor destructor, int startSize) : base(reference, reference.GetType(), constructor, destructor, startSize, true) { }

		protected Pool(object reference, Type type, Constructor<object> constructor, Destructor destructor, int startSize, bool initialize) : base(reference, type, constructor, destructor, startSize, initialize) { }

		public virtual T Create()
		{
			return (T)base.CreateObject();
		}

		protected override object CreateObject()
		{
			return Create();
		}
	}

	namespace Internal.Pool
	{
		public abstract class Pool : IPool
		{
			public delegate T Constructor<out T>();
			public delegate void Destructor(object instance);

			static readonly List<Pool> toUpdate = new List<Pool>(4);
			static Thread updadeThread;

			public Type Type { get; protected set; }
			public int Size
			{
				get { return hashedInstances.Count; }
			}

			protected readonly object reference;
			protected readonly Constructor<object> constructor;
			protected readonly Destructor destructor;
			protected readonly int startSize;
			protected readonly bool isPoolable;
			protected readonly bool isInitializable;
			protected readonly bool isSettersInitializable;
			protected readonly HashSet<object> hashedInstances;
			protected readonly Queue<object> instances;
			protected readonly Queue<object> toInitialize;
			protected List<IPoolSetter> setters;
			protected bool updating;

			protected Pool(object reference, Type type, Constructor<object> constructor, Destructor destructor, int startSize, bool initialize)
			{
				PoolUtility.InitializeJanitor();

				this.reference = reference;
				this.constructor = constructor ?? Construct;
				this.destructor = destructor ?? Destroy;
				this.startSize = startSize;

				Type = type;
				isPoolable = reference is IPoolable;
				isInitializable = reference is IPoolInitializable;
				isSettersInitializable = reference is IPoolSettersInitializable;
				hashedInstances = new HashSet<object>();
				instances = new Queue<object>(startSize);
				toInitialize = new Queue<object>(startSize);

				if (initialize)
					Initialize();
			}

			public virtual T CreateCopy<T>(T reference) where T : class, ICopyable
			{
				var instance = (T)CreateObject();
				instance.Copy(reference);

				return instance;
			}

			public virtual void Recycle(object instance)
			{
				if (instance == null)
					return;

				if (instance.GetType() != Type)
					throw new ArgumentException(string.Format("The type of the instance ({0}) doesn't match the pool type ({1}).", instance.GetType().Name, Type.Name));

				if (hashedInstances.Contains(instance))
					return;

				if (isPoolable)
					((IPoolable)instance).OnRecycle();

				Enqueue(instance, true);
				Subscribe(this);
			}

			public virtual void RecycleElements<T>(T elements) where T : class, IList
			{
				if (elements == null)
					return;

				for (int i = 0; i < elements.Count; i++)
					Recycle(elements[i]);

				elements.Clear();
			}

			public virtual bool Contains(object instance)
			{
				return hashedInstances.Contains(instance);
			}

			public virtual void Clear()
			{
				lock (instances) instances.Clear();
				lock (toInitialize) toInitialize.Clear();

				var enumerator = hashedInstances.GetEnumerator();

				while (enumerator.MoveNext())
					destructor(enumerator.Current);

				enumerator.Dispose();

				hashedInstances.Clear();
				Unsubscribe(this);
			}

			public virtual void Reset()
			{
				Initialize();

				lock (toInitialize)
				{
					lock (instances)
					{
						while (instances.Count > 0)
							toInitialize.Enqueue(instances.Dequeue());
					}
				}
			}

			protected void Initialize()
			{
				if (isSettersInitializable)
					((IPoolSettersInitializable)reference).OnPrePoolSettersInitialize();

				if (setters == null)
					setters = PoolUtility.GetSetters(reference);
				else
					lock (setters) setters = PoolUtility.GetSetters(reference);

				if (isSettersInitializable)
					((IPoolSettersInitializable)reference).OnPostPoolSettersInitialize(setters);

				while (Size < startSize)
					Enqueue(CreateInstance(), false);
			}

			protected virtual object CreateObject()
			{
				var instance = GetInstance();

				if (isPoolable)
					((IPoolable)instance).OnCreate();

				Subscribe(this);

				return instance;
			}

			protected virtual object GetInstance()
			{
				object instance;

				do
				{
					if (instances.Count > 0)
						instance = Dequeue();
					else
						instance = CreateInstance();
				}
				while (instance == null);

				return instance;
			}

			protected virtual object CreateInstance()
			{
				var instance = constructor();

				if (isInitializable)
					((IPoolInitializable)instance).OnPrePoolInitialize();

				PoolUtility.InitializeFields(instance, setters);

				if (isInitializable)
					((IPoolInitializable)instance).OnPostPoolInitialize();

				return instance;
			}

			protected virtual void Enqueue(object instance, bool initialize)
			{
				if (!hashedInstances.Add(instance))
					return;

				if (initialize && setters.Count > 0)
					lock (toInitialize) { toInitialize.Enqueue(instance); }
				else
					lock (instances) { instances.Enqueue(instance); }
			}

			protected virtual object Dequeue()
			{
				object instance;
				lock (instances) { instance = instances.Dequeue(); }
				hashedInstances.Remove(instance);

				return instance;
			}

			protected virtual object Construct()
			{
				return Activator.CreateInstance(Type);
			}

			protected virtual void Destroy(object instance) { }

			object IPool.Create()
			{
				return CreateObject();
			}

			protected static void Subscribe(Pool pool)
			{
				InitializeUpdateThread();

				if (pool.updating)
					return;

				pool.updating = true;
				toUpdate.Add(pool);
			}

			protected static void Unsubscribe(Pool pool)
			{
				pool.updating = false;
			}

			static void InitializeUpdateThread()
			{
				if (updadeThread == null || updadeThread.ThreadState == ThreadState.Stopped)
				{
					updadeThread = new Thread(UpdateAsync);
					updadeThread.Start();
				}
			}

			static void UpdateAsync()
			{
				try
				{
					while (ApplicationUtility.IsPlaying)
					{
						for (int i = toUpdate.Count - 1; i >= 0; i--)
						{
							Pool pool;

							lock (toUpdate)
							{
								if (toUpdate.Count > i)
								{
									pool = toUpdate[i];

									if (pool == null || !pool.updating)
									{
										toUpdate.RemoveAt(i);
										continue;
									}
								}
								else
									break;
							}

							UpdatePoolAsync(pool);
						}

						Thread.Sleep(100);
					}
				}
				catch (Exception e) { Debug.LogError(e); }
			}

			static void UpdatePoolAsync(Pool pool)
			{
				int count = Mathf.Max(pool.Size / 10, 1);

				for (int i = 0; i < count; i++)
				{
					object instance;

					lock (pool.toInitialize)
					{
						if (pool.toInitialize.Count > 0)
							instance = pool.toInitialize.Dequeue();
						else
						{
							Unsubscribe(pool);
							return;
						}
					}

					if (pool.isInitializable)
						((IPoolInitializable)instance).OnPrePoolInitialize();

					lock (pool.setters) PoolUtility.InitializeFields(instance, pool.setters);
					lock (pool.instances) pool.instances.Enqueue(instance);

					if (pool.isInitializable)
						((IPoolInitializable)instance).OnPostPoolInitialize();
				}
			}
		}
	}
}