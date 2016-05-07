using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Pooling2.Internal
{
	public abstract class PoolBase<T> : IPool<T> where T : class
	{
		public int Size
		{
			get { return instances.Count; }
			set { SetSize(value); }
		}

		Type IPool.Type
		{
			get { return typeof(T); }
		}

		protected readonly Queue<T> instances = new Queue<T>();
		protected readonly HashSet<T> hashedInstances = new HashSet<T>();

		public T Create()
		{
			T instance;

			if (instances.Count > 0)
				instance = Dequeue();
			else
				instance = Construct();

			Initialize(instance);

			return instance;
		}

		public void Recycle(T instance)
		{
			Enqueue(instance);
		}

		public bool Contains(T instance)
		{
			return hashedInstances.Contains(instance);
		}

		public void Clear()
		{
			instances.Clear();
			hashedInstances.Clear();
		}

		protected virtual void Enqueue(T instance)
		{
			if (instance == null)
				return;

			if (hashedInstances.Add(instance))
				instances.Enqueue(instance);
			else
				throw new ArgumentException(string.Format("Instance {0} is already in the pool.", instance));
		}

		protected virtual T Dequeue()
		{
			var instance = instances.Dequeue();
			hashedInstances.Remove(instance);

			return instance;
		}

		void SetSize(int size)
		{
			while (instances.Count > size)
				instances.Dequeue();

			while (instances.Count < size)
				instances.Enqueue(Construct());
		}

		object IPool.Create()
		{
			return Create();
		}

		void IPool.Recycle(object instance)
		{
			Recycle(instance as T);
		}

		bool IPool.Contains(object instance)
		{
			return Contains(instance as T);
		}

		protected abstract T Construct();
		protected abstract void Initialize(T instance);
	}
}
