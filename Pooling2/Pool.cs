using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Pooling2.Internal;

namespace Pseudo.Pooling2
{
	public class Pool<T> : IPool<T> where T : class
	{
		public IStorage<T> Storage
		{
			get { return storage; }
		}
		public IInitializer<T> Initializer
		{
			get { return initializer; }
		}

		Type IPool.Type
		{
			get { return typeof(T); }
		}
		IStorage IPool.Storage
		{
			get { return storage; }
		}
		IInitializer IPool.Initializer
		{
			get { return initializer; }
		}

		readonly Func<T> factory;
		readonly IStorage<T> storage;
		readonly IInitializer<T> initializer;

		public Pool(Func<T> factory, IStorage<T> storage = null, IInitializer<T> initializer = null)
		{
			this.factory = factory;
			this.storage = storage ?? new Storage<T>(this.factory);
			this.initializer = initializer ?? Initializer<T>.Default;
		}

		public T Create()
		{
			var instance = storage.Count > 0 ? storage.Take() : factory();
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
