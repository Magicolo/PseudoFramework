using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo.Internal
{
	public abstract class PoolManagerBase<TItem, TId, TKey, TPool> where TItem : class where TId : class where TPool : PoolBase<TItem>
	{
		protected readonly Dictionary<TKey, TPool> pools = new Dictionary<TKey, TPool>();

		public abstract void Recycle(TItem item);

		public virtual void Recycle<TD>(ref TD item) where TD : class, TItem
		{
			Recycle(item);
			item = null;
		}

		public virtual void RecycleElements(IList<TItem> array)
		{
			if (array == null)
				return;

			for (int i = 0; i < array.Count; i++)
				Recycle(array[i]);

			array.Clear();
		}

		public virtual TPool GetPool(TId identifier)
		{
			TPool pool;
			TKey key = GetPoolKey(identifier);

			if (!pools.TryGetValue(key, out pool))
			{
				pool = CreatePool(identifier);
				pools[key] = pool;
			}

			return pool;
		}

		public bool ContainsPool(TId identifier)
		{
			return pools.ContainsKey(GetPoolKey(identifier));
		}

		public bool ContainsItem(TItem item)
		{
			foreach (TPool pool in pools.Values)
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

			foreach (TPool pool in pools.Values)
				itemCount += pool.Count();

			return itemCount;
		}

		public void Clear()
		{
			foreach (TPool pool in pools.Values)
				pool.Clear();

			pools.Clear();
		}

		protected abstract TKey GetPoolKey(TId identifier);

		protected abstract TPool CreatePool(TId identifier);
	}
}