using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal;

namespace Pseudo.Internal
{
	public abstract class PrefabPoolManager<T> : PoolManagerBase<T, T, Object, PrefabPool<T>> where T : Object
	{
		protected static readonly CachedValue<GameObject> poolHolder = new CachedValue<GameObject>(() => new GameObject("Pools"));

		protected readonly Dictionary<Object, PrefabPool<T>> instancePool = new Dictionary<Object, PrefabPool<T>>();
		protected readonly CachedValue<GameObject> cachedGameObject;
		protected readonly CachedValue<Transform> cachedTransform;

		public GameObject GameObject { get { return cachedGameObject; } }
		public Transform Transform { get { return cachedTransform; } }

		protected PrefabPoolManager()
		{
			cachedGameObject = new CachedValue<GameObject>(() => poolHolder.Value.AddChild(typeof(T).Name));
			cachedTransform = new CachedValue<Transform>(() => cachedGameObject.Value.transform);
		}

		public virtual TD Create<TD>(TD prefab) where TD : class, T
		{
			if (prefab == null)
				return null;

			PrefabPool<T> pool = GetPool(prefab);
			TD item = (TD)pool.Create();
			instancePool[GetPoolKey(item)] = pool;

			return item;
		}

		public virtual TD Create<TD>(TD prefab, Vector3 position, Transform parent = null) where TD : class, T
		{
			if (prefab == null)
				return null;

			PrefabPool<T> pool = GetPool(prefab);
			TD item = (TD)pool.Create(position, parent);
			instancePool[GetPoolKey(item)] = pool;

			return item;
		}

		public override void Recycle(T item)
		{
			if (item == null)
				return;

			PrefabPool<T> pool;

			if (instancePool.TryGetValue(GetPoolKey(item), out pool))
				pool.Recycle(item);
		}
	}
}