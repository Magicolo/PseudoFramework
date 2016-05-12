using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Pooling2.Internal
{
	public class Storage<T> : IStorage<T> where T : class
	{
		public int Count
		{
			get { return instances.Count; }
		}
		public int Capacity
		{
			get { return capacity; }
			set
			{
				capacity = value;
				Trim(capacity);
			}
		}

		readonly Queue<T> instances = new Queue<T>();
		readonly HashSet<T> hashedInstances = new HashSet<T>();
		int capacity = 1024;

		public T Take()
		{
			T instance = null;

			if (instances.Count > 0)
			{
				instance = instances.Dequeue();
				hashedInstances.Remove(instance);
			}

			return instance;
		}

		public bool Put(T instance)
		{
			if (Count < Capacity && hashedInstances.Add(instance))
			{
				instances.Enqueue(instance);
				return true;
			}
			else
				return false;
		}

		public void Fill(int count, Func<T> factory)
		{
			while (Count < count && Put(factory())) { }
		}

		public void Trim(int count)
		{
			while (Count > count && Take() != null) { }
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
	}
}
