using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal;
using System;
using Pseudo.Internal.Pool;

namespace Pseudo
{
	public delegate T Constructor<out T>();
	public delegate void Destructor(object instance);

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
			public Type Type { get; private set; }
			public int Size
			{
				get { return hashedInstances.Count; }
			}

			protected readonly object reference;

			readonly Constructor<object> constructor;
			readonly Destructor destructor;
			readonly int startSize;
			readonly bool isPoolable;
			readonly HashSet<object> hashedInstances;
			IPoolUpdater updater;
			bool updating;

			protected Pool(object reference, Type type, Constructor<object> constructor, Destructor destructor, int startSize, bool initialize)
			{
				PoolUtility.InitializeJanitor();

				this.reference = reference;
				this.constructor = constructor ?? Construct;
				this.destructor = destructor ?? Destroy;
				this.startSize = startSize;

				Type = type;
				isPoolable = reference is IPoolable;
				hashedInstances = new HashSet<object>();

				if (ApplicationUtility.IsMultiThreaded)
					updater = new AsyncPoolUpdater();
				else
					updater = new SyncPoolUpdater();

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
				updater.Update();
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
				updater.Clear();

				var enumerator = hashedInstances.GetEnumerator();

				while (enumerator.MoveNext())
					destructor(enumerator.Current);

				enumerator.Dispose();

				hashedInstances.Clear();
			}

			public virtual void Reset()
			{
				Initialize();
				updater.Reset();
			}

			protected void Initialize()
			{
				updater.Initializer = PoolUtility.GetPoolInitializer(reference);

				while (Size < startSize)
					Enqueue(CreateInstance(), false);
			}

			protected virtual object CreateObject()
			{
				var instance = GetInstance();

				if (isPoolable)
					((IPoolable)instance).OnCreate();

				updater.Update();

				return instance;
			}

			protected virtual object GetInstance()
			{
				var instance = Dequeue();

				if (instance == null)
					instance = CreateInstance();

				return instance;
			}

			protected virtual object CreateInstance()
			{
				var instance = constructor();
				updater.Initializer.InitializeFields(instance);

				return instance;
			}

			protected virtual void Enqueue(object instance, bool initialize)
			{
				if (hashedInstances.Add(instance))
					updater.Enqueue(instance, initialize);
			}

			protected virtual object Dequeue()
			{
				var instance = updater.Dequeue();
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
		}
	}
}