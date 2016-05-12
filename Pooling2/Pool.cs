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
		public int Count
		{
			get { return storage.Count; }
			set
			{
				storage.Capacity = value > storage.Capacity ? value : storage.Capacity;
				storage.Trim(value);
				storage.Fill(value, factory);
			}
		}
		public int Capacity
		{
			get { return storage.Capacity; }
			set { storage.Capacity = value; }
		}

		Type IPool.Type
		{
			get { return typeof(T); }
		}

		readonly Func<T> factory;
		readonly IStorage<T> storage;
		readonly IInitializer<T> initializer;

		public Pool(Func<T> factory, IStorage<T> storage = null, IInitializer<T> initializer = null)
		{
			this.factory = factory;
			this.storage = storage ?? new Storage<T>();
			this.initializer = initializer ?? Initializer<T>.Default;
		}

		public T Create()
		{
			var instance = storage.Take() ?? factory();
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

		public bool Contains(T instance)
		{
			return storage.Contains(instance);
		}

		public void Clear()
		{
			storage.Clear();
		}

		object IPool.Create()
		{
			return Create();
		}

		bool IPool.Recycle(object instance)
		{
			return Recycle(instance as T);
		}

		bool IPool.Contains(object instance)
		{
			return Contains(instance as T);
		}
	}
}
