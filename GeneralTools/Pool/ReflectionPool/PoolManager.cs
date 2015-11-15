using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using System.Reflection;
using System.Threading;

namespace Pseudo
{
	public static class PoolManager
	{
		static readonly Dictionary<PMonoBehaviour, BehaviourPool> pools = new Dictionary<PMonoBehaviour, BehaviourPool>(16);
		static readonly CachedValue<GameObject> cachedGameObject;
		static readonly CachedValue<Transform> cachedTransform;

		public static GameObject GameObject { get { return cachedGameObject; } }
		public static Transform Transform { get { return cachedTransform; } }

		static PoolManager()
		{
			cachedGameObject = new CachedValue<GameObject>(() => new GameObject("Pool Manager"));
			cachedTransform = new CachedValue<Transform>(() => cachedGameObject.Value.transform);
		}

		public static T Create<T>(T prefab) where T : PMonoBehaviour
		{
			return (T)Create((PMonoBehaviour)prefab);
		}

		public static PMonoBehaviour Create(PMonoBehaviour prefab)
		{
			BehaviourPool pool = GetPool(prefab);
			PMonoBehaviour instance = pool.Create();

			return instance;
		}

		public static void Recycle(PMonoBehaviour instance)
		{
			if (instance == null)
				return;

			if (instance.Prefab == null)
				instance.CachedGameObject.Destroy();
			else
				GetPool(instance.Prefab).Recycle(instance);
		}

		public static BehaviourPool GetPool(PMonoBehaviour prefab)
		{
			BehaviourPool pool;

			if (!pools.TryGetValue(prefab, out pool))
				pool = CreatePool(prefab);

			return pool;
		}

		public static bool Contains(PMonoBehaviour prefab)
		{
			return pools.ContainsKey(prefab);
		}

		public static int Count()
		{
			return pools.Count;
		}

		public static void Clear()
		{
			foreach (var pool in pools)
				pool.Value.Clear();

			pools.Clear();
		}

		static BehaviourPool CreatePool(PMonoBehaviour prefab)
		{
			BehaviourPool pool = new BehaviourPool(prefab);
			pool.Transform.parent = Transform;
			pools[prefab] = pool;

			return pool;
		}
	}
}