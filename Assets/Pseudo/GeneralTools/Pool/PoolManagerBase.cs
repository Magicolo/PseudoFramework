using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo.Internal
{
	public abstract class PoolManagerBase<T, TI, TK, TP> where T : class where TI : class where TP : PoolBase<T>
	{
		protected readonly Dictionary<TK, TP> pools = new Dictionary<TK, TP>();

		public abstract void Recycle(T item);

		public virtual void Recycle<TD>(ref TD item) where TD : class, T
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

		public virtual TP GetPool(TI identifier)
		{
			TP pool;
			TK key = GetPoolKey(identifier);

			if (!pools.TryGetValue(key, out pool))
			{
				pool = CreatePool(identifier);
				pools[key] = pool;
			}

			return pool;
		}

		public bool ContainsPool(TI identifier)
		{
			return pools.ContainsKey(GetPoolKey(identifier));
		}

		public bool ContainsItem(T item)
		{
			foreach (TP pool in pools.Values)
			{
				if (pool.Contains(item))
					return true;
			}

			return false;
		}

		public int PoolCount()
		{
			return pools.Count;
		}

		public int ItemCount()
		{
			int itemCount = 0;

			foreach (TP pool in pools.Values)
				itemCount += pool.Count();

			return itemCount;
		}

		public void Clear()
		{
			foreach (TP pool in pools.Values)
				pool.Clear();

			pools.Clear();
		}

		protected abstract TK GetPoolKey(TI identifier);

		protected abstract TP CreatePool(TI identifier);
	}
}