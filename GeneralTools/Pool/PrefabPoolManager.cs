using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal;
using Pseudo.Internal.Pool;

namespace Pseudo
{
	public static class PrefabPoolManager
	{
		public static int StartSize = 2;

		static readonly Dictionary<object, Pool> pools = new Dictionary<object, Pool>(8);
		static readonly Dictionary<object, Pool> instancePool = new Dictionary<object, Pool>(64);

		public static T Create<T>(T prefab) where T : class
		{
			if (prefab == null)
				return null;

			var pool = GetPool(prefab);
			var instance = (T)pool.Create();
			instancePool[instance] = pool;

			return instance;
		}

		public static void Recycle(object instance)
		{
			if (instance == null)
				return;

			Pool pool;

			if (instancePool.TryGetValue(instance, out pool))
				pool.Recycle(instance);
			else if (instance is Component)
				((Component)instance).gameObject.Destroy();
			else if (instance is UnityEngine.Object)
				((UnityEngine.Object)instance).Destroy();
		}

		public static void Recycle<T>(ref T instance) where T : class
		{
			Recycle(instance);
			instance = null;
		}

		public static Pool GetPool(object prefab)
		{
			Pool pool;

			if (!pools.TryGetValue(prefab, out pool))
			{
				pool = PoolUtility.CreatePool(prefab, StartSize);
				pools[prefab] = pool;
			}

			return pool;
		}

		public static int PoolCount()
		{
			return pools.Count;
		}

		public static void ClearPools()
		{
			foreach (var pool in pools)
				pool.Value.Clear();

			pools.Clear();
			instancePool.Clear();
		}
	}
}