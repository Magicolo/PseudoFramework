using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo
{
	public class PoolManager : Singleton<PoolManager>
	{
		public Pool[] Pools = new Pool[0];
		public event System.Action<Object> OnCreate;
		public event System.Action<Object> OnRecycle;

		readonly Dictionary<Object, Pool> pools = new Dictionary<Object, Pool>();
		readonly Dictionary<Object, Object> instancePrefabs = new Dictionary<Object, Object>();

		protected override void Awake()
		{
			base.Awake();

			for (int i = 0; i < Pools.Length; i++)
			{
				Pool pool = Instantiate(Pools[i]);
				pool.CachedTransform.parent = CachedTransform;
				pools[pool.Prefab] = pool;
			}
		}

		public T Create<T>(T prefab, Vector3 position = default(Vector3), Transform parent = null) where T : Object
		{
			T item = GetPool(prefab).Create<T>(position, parent);
			instancePrefabs[item] = prefab;
			RaiseOnCreateEvent(item);

			return item;
		}

		public void Recycle<T>(T item) where T : Object
		{
			if (item == null)
				return;

			Object prefab;

			if (!instancePrefabs.TryGetValue(item, out prefab))
				return;

			GetPool(prefab).Recycle(item);
			RaiseOnRecycleEvent(item);
		}

		public void Recycle<T>(ref T item) where T : Object
		{
			Recycle(item);

			item = null;
		}

		public Pool GetPool(Object prefab)
		{
			Pool pool;

			if (!pools.TryGetValue(prefab, out pool))
			{
				GameObject poolGameObject = CachedGameObject.AddChild(prefab.name);
				pool = poolGameObject.AddComponent<Pool>();
				pool.Initialize(prefab, 0);
				pools[prefab] = pool;
			}

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