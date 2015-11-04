using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo.Internal
{
	public abstract class PoolBase<T> where T : class
	{
		protected readonly int startCount;
		protected readonly Queue<T> pool;

		protected PoolBase(int startCount = 4)
		{
			this.startCount = startCount;

			pool = new Queue<T>(startCount);
		}

		public virtual T Create()
		{
			T item = GetItem();

			return item;
		}

		public virtual TC CreateCopy<TC>(TC reference) where TC : class, T, ICopyable<TC>
		{
			TC item = (TC)GetItem();

			if (reference != null)
				item.Copy(reference);

			return item;
		}

		public virtual void CreateElements<TC>(IList<TC> array) where TC : class, T, ICopyable<TC>
		{
			if (array == null)
				return;

			for (int i = 0; i < array.Count; i++)
				array[i] = CreateCopy(array[i]);
		}

		public virtual void Recycle(T item)
		{
			if (item == null)
				return;

			Enqueue(item);
		}

		public virtual void Recycle(ref T item)
		{
			Recycle(item);
			item = null;
		}

		public virtual void RecycleElements(IList<T> array)
		{
			if (array == null)
				return;

			for (int i = 0; i < array.Count; i++)
				Recycle(array[i]);

			array.Clear();
		}

		public virtual bool Contains(T item)
		{
			return pool.Contains(item);
		}

		public virtual int Count()
		{
			return pool.Count;
		}

		public virtual void Clear()
		{
			pool.Clear();
		}

		protected void Initialize()
		{
			for (int i = 0; i < startCount; i++)
				Enqueue(CreateItem());
		}

		protected virtual T GetItem()
		{
			T item = null;

			while (item == null)
			{
				if (CanDequeue())
					item = Dequeue();
				else
					item = CreateItem();
			}

			return item;
		}

		protected abstract T CreateItem();

		protected virtual void Enqueue(T item)
		{
			pool.Enqueue(item);
		}

		protected virtual T Dequeue()
		{
			return pool.Dequeue();
		}

		protected virtual bool CanDequeue()
		{
			return pool.Count > 0;
		}
	}
}