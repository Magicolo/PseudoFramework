using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Pooling.Internal;

namespace Pseudo.Pooling
{
	public class Pool<T> : IPool<T> where T : class
	{
		public IFactory<T> Factory
		{
			get { return factory; }
		}
		public IInitializer<T> Initializer
		{
			get { return initializer; }
		}
		public IStorage<T> Storage
		{
			get { return storage; }
		}

		Type IPool.Type
		{
			get { return typeof(T); }
		}
		IFactory IPool.Factory
		{
			get { return factory; }
		}
		IInitializer IPool.Initializer
		{
			get { return initializer; }
		}
		IStorage IPool.Storage
		{
			get { return storage; }
		}

		readonly IFactory<T> factory;
		readonly IInitializer<T> initializer;
		readonly IStorage<T> storage;

		public Pool(IFactory<T> factory = null, IInitializer<T> initializer = null, IStorage<T> storage = null)
		{
			this.factory = factory ?? new DefaultFactory<T>();
			this.initializer = initializer ?? Initializer<T>.Default;
			this.storage = storage ?? new Storage<T>(this.factory);
		}

		public Pool(Func<T> factory, IInitializer<T> initializer = null, IStorage<T> storage = null)
			: this(factory == null ? null : new MethodFactory<T>(factory), initializer, storage) { }

		public Pool(Func<T> factory, Action<T> initializer, IStorage<T> storage = null)
			: this(factory, initializer == null ? null : new MethodInitializer<T>(initializer), storage) { }

		public T Create()
		{
			T instance;

			do
			{
				if (storage.Count > 0)
					instance = storage.Take();
				else
				{
					instance = factory.Create();
					break;
				}
			}
			while (instance == null);

			initializer.OnCreate(instance);

			return instance;
		}

		public bool Recycle(T instance)
		{
			if (instance == null)
				return false;

			initializer.OnRecycle(instance);

			return storage.Put(instance);
		}

		object IPool.Create()
		{
			return Create();
		}

		bool IPool.Recycle(object instance)
		{
			return Recycle(instance as T);
		}
	}
}
