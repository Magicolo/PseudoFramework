using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo
{
	public class PoolManager : Singleton<PoolManager>
	{
		public event System.Action<Object> OnCreate;
		public event System.Action<Object> OnRecycle;

		readonly Dictionary<Object, PrefabPool> pools = new Dictionary<Object, PrefabPool>();
		readonly Dictionary<Object, Object> instancePrefabs = new Dictionary<Object, Object>();

		protected override void Awake()
		{
			base.Awake();

			PrefabPool[] childPools = GetComponentsInChildren<PrefabPool>();

			for (int i = 0; i < childPools.Length; i++)
			{
				PrefabPool childPool = childPools[i];
				pools[childPool.Prefab] = childPool;
			}
		}

		public T Create<T>(T prefab, Transform parent = null, Vector3 position = default(Vector3)) where T : Object
		{
			PrefabPool pool = GetPool(prefab);
			T item = pool.Create<T>(parent, position);
			instancePrefabs[item] = prefab;
			RaiseOnCreateEvent(item);

			return item;
		}

		public void Recycle<T>(T item) where T : Object
		{
			if (item == null)
				return;

			if (!instancePrefabs.ContainsKey(item))
				return;

			Object prefab = instancePrefabs[item];
			PrefabPool pool = GetPool(prefab);
			pool.Recycle(item);
			RaiseOnRecycleEvent(item);
		}

		public void Recycle<T>(ref T item) where T : Object
		{
			Recycle(item);

			item = null;
		}

		public PrefabPool GetPool(Object prefab)
		{
			PrefabPool pool;

			if (!pools.ContainsKey(prefab))
			{
				GameObject poolGameObject = CachedGameObject.AddChild(prefab.name);
				pool = poolGameObject.AddComponent<PrefabPool>();
				pool.Initialize(prefab, 0);
				pools[prefab] = pool;
			}
			else
				pool = pools[prefab];

			return pool;
		}

		void RaiseOnCreateEvent(Object item)
		{
			if (OnCreate != null)
				OnCreate(item);
		}

		void RaiseOnRecycleEvent(Object item)
		{
			if (OnRecycle != null)
				OnRecycle(item);
		}
	}
}