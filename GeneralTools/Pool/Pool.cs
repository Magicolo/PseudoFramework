using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using System.Reflection;
using System.Threading;
using Pseudo.Internal;
using System;
using System.Runtime.Serialization;

namespace Pseudo.Internal.Pool
{
	public class Pool<T> : Pool where T : class
	{
		public Pool(T reference, int startSize = 8) : base(reference, reference.GetType(), startSize) { }

		new public virtual T Create()
		{
			return (T)base.Create();
		}
	}

	public class Pool
	{
		static readonly List<Pool> toUpdate = new List<Pool>(16);
		static Thread updadeThread;

		public Type Type { get; protected set; }
		public int StartSize { get; protected set; }
		public int Size { get; protected set; }

		protected readonly object reference;
		protected readonly bool isPoolable;
		protected readonly Queue instances;
		protected readonly Queue toInitialize;
		protected List<IPoolSetter> setters;
		protected bool updating;

		static Pool()
		{
			updadeThread = new Thread(UpdateAsync);
			updadeThread.Start();
		}

		public Pool(Type type, int startSize = 8) : this(Activator.CreateInstance(type), type, startSize) { }

		public Pool(object reference, int startSize = 8) : this(reference, reference.GetType(), startSize) { }

		protected Pool(object reference, Type type, int startSize)
		{
			this.reference = reference;
			Type = type;
			StartSize = startSize;
			isPoolable = reference is IPoolable;
			instances = new Queue(startSize);
			toInitialize = new Queue(startSize);
			Initialize();
		}

		public virtual object Create()
		{
			var instance = GetInstance();

			if (isPoolable)
				((IPoolable)instance).OnCreate();

			Subscribe(this);

			return instance;
		}

		public virtual T CreateCopy<T>(T reference) where T : class, ICopyable
		{
			var instance = (T)Create();
			instance.Copy(reference);

			return instance;
		}

		public virtual void Recycle(object instance)
		{
			if (instance == null)
				return;

			if (instance.GetType() != Type)
				throw new TypeMismatchException(string.Format("The type of the instance ({0}) doesn't match the pool type ({1}).", instance.GetType().Name, Type.Name));

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
			lock (instances)
			{
				if (instances.Contains(instance))
					return true;
			}

			lock (toInitialize)
			{
				if (toInitialize.Contains(instance))
					return true;
			}

			return false;
		}

		public virtual void Clear()
		{
			lock (instances) { instances.Clear(); }
			lock (toInitialize) { toInitialize.Clear(); }
			Unsubscribe(this);
		}

		void Initialize()
		{
			bool isInitializable = reference is IPoolInitializable;

			if (isInitializable)
				((IPoolInitializable)reference).OnBeforePoolInitialize();

			setters = PoolUtility.GetSetters(reference);

			if (isInitializable)
				((IPoolInitializable)reference).OnAfterPoolInitialize(setters);

			while (Size < StartSize)
				Enqueue(CreateInstance(), false);
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
			var instance = Activator.CreateInstance(Type);
			PoolUtility.InitializeFields(instance, setters);

			return instance;
		}

		protected virtual void Enqueue(object instance, bool initialize)
		{
			if (initialize && setters.Count > 0)
				lock (toInitialize) { toInitialize.Enqueue(instance); }
			else
				lock (instances) { instances.Enqueue(instance); }

			Size++;
		}

		protected virtual object Dequeue()
		{
			object instance;
			lock (instances) { instance = instances.Dequeue(); }
			Size--;

			return instance;
		}

		protected static void Subscribe(Pool pool)
		{
			if (pool.updating)
				return;

			pool.updating = true;
			toUpdate.Add(pool);
		}

		protected static void Unsubscribe(Pool pool)
		{
			if (!pool.updating)
				return;

			pool.updating = false;
		}

		static void UpdateAsync()
		{
			try
			{
				while (true)
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
								return;
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

				PoolUtility.InitializeFields(instance, pool.setters);
				lock (pool.instances) pool.instances.Enqueue(instance);
			}
		}
	}

	public class TypeMismatchException : Exception
	{
		public TypeMismatchException(string message) : base(message) { }
	}
}