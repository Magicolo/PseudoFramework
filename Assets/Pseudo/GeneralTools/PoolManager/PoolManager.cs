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

		protected readonly Dictionary<GameObject, Pool> pools = new Dictionary<GameObject, Pool>();
		protected readonly Dictionary<Object, Pool> instancePool = new Dictionary<Object, Pool>();

		protected override void Awake()
		{
			base.Awake();

			for (int i = 0; i < Pools.Length; i++)
			{
				Pool pool = Instantiate(Pools[i]);
				pool.CachedTransform.parent = CachedTransform;
				pools[GetGameObject(pool.Prefab)] = pool;
			}
		}

		public virtual T Create<T>(T prefab, Vector3 position = default(Vector3), Transform parent = null) where T : Object
		{
			Pool pool = GetPool(prefab);
			T item = pool.Create<T>(position, parent);
			instancePool[item] = pool;

			return item;
		}

		public virtual void Recycle<T>(T item) where T : Object
		{
			if (item == null)
				return;

			Pool pool;

			if (!instancePool.TryGetValue(item, out pool))
				return;

			pool.Recycle(item);
		}

		public virtual void Recycle<T>(ref T item) where T : Object
		{
			Recycle(item);

			item = null;
		}

		public virtual Pool GetPool(Object prefab)
		{
			Pool pool;
			GameObject prefabGameObject = GetGameObject(prefab);

			if (!pools.TryGetValue(prefabGameObject, out pool))
			{
				GameObject poolGameObject = CachedGameObject.AddChild(prefab.name);
				pool = poolGameObject.AddComponent<Pool>();
				pool.Initialize(prefab, 0);
				pools[prefabGameObject] = pool;
			}

			return pool;
		}

		public static GameObject GetGameObject(Object item)
		{
			GameObject itemGameObject = item as GameObject;

			if (itemGameObject == null)
				itemGameObject = (item as Component).gameObject;

			return itemGameObject;
		}
	}
}
