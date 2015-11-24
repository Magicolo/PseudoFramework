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
		static readonly Dictionary<object, Pool> pools = new Dictionary<object, Pool>(16);
		static readonly Dictionary<object, Pool> instancePool = new Dictionary<object, Pool>(256);
		static readonly CachedValue<GameObject> cachedGameObject;
		static readonly CachedValue<Transform> cachedTransform;

		public static GameObject GameObject { get { return cachedGameObject; } }
		public static Transform Transform { get { return cachedTransform; } }
		public static int StartSize = 8;

		static PoolManager()
		{
			cachedGameObject = new CachedValue<GameObject>(() => new GameObject("Pool Manager"));
			cachedTransform = new CachedValue<Transform>(() => cachedGameObject.Value.transform);
		}

		public static T Create<T>(T reference) where T : class
		{
			return (T)Create((object)reference);
		}

		public static object Create(object reference)
		{
			var pool = GetPool(reference);
			var instance = pool.Create();
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
			else if (instance is Object)
				((Object)instance).Destroy();

			//if (instance.Prefab == null)
			//	instance.CachedGameObject.Destroy();
			//else
			//	GetPool(instance.Prefab).Recycle(instance);
		}

		public static Pool GetPool(object reference)
		{
			Pool pool;

			if (!pools.TryGetValue(reference, out pool))
				pool = CreatePool(reference);

			return pool;
		}

		public static bool Contains(object reference)
		{
			return pools.ContainsKey(reference);
		}

		public static int PoolCount()
		{
			return pools.Count;
		}

		public static void Clear()
		{
			foreach (var pool in pools)
				pool.Value.Clear();

			pools.Clear();
		}

		static Pool CreatePool(object reference)
		{
			Pool pool;

			if (reference is Component)
			{
				var behaviourPool = new ComponentPool((Component)reference, StartSize);
				behaviourPool.Transform.parent = Transform;
				pool = behaviourPool;
			}
			else if (reference is GameObject)
			{
				var gameObjectPool = new GameObjectPool((GameObject)reference, StartSize);
				gameObjectPool.Transform.parent = Transform;
				pool = gameObjectPool;
			}
			else if (reference is ScriptableObject)
				pool = new ScriptablePool((ScriptableObject)reference, StartSize);
			else
				pool = new Pool(reference, StartSize);

			pools[reference] = pool;

			return pool;
		}
	}
}