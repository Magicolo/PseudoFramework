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

		static readonly Dictionary<object, IPool> pools = new Dictionary<object, IPool>(8);
		static readonly Dictionary<object, IPool> instancePool = new Dictionary<object, IPool>(64);

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

			IPool pool;

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

		public static IPool GetPool(object prefab)
		{
			IPool pool;

			if (!pools.TryGetValue(prefab, out pool))
			{
				pool = PoolUtility.CreatePrefabPool(prefab, ApplicationUtility.IsPlaying ? StartSize : 0);
				pools[prefab] = pool;
			}

			return pool;
		}

		public static int PoolCount()
		{
			return pools.Count;
		}

		public static bool HasPool(object prefab)
		{
			return pools.ContainsKey(prefab);
		}

		public static void ClearPool(object prefab)
		{
			IPool pool;

			if (pools.Pop(prefab, out pool))
				pool.Clear();
		}

		public static void ClearPools()
		{
			foreach (var pool in pools)
				pool.Value.Clear();

			pools.Clear();
			instancePool.Clear();
		}

		public static void ResetPool(object prefab)
		{
			IPool pool;

			if (pools.TryGetValue(prefab, out pool))
				pool.Reset();
		}

		public static void ResetPools()
		{
			foreach (var pool in pools)
				pool.Value.Reset();
		}

#if UNITY_EDITOR
		[UnityEditor.Callbacks.DidReloadScripts]
		static void OnReloadScripts()
		{
			Pseydo.Internal.Editor.InspectorUtility.OnValidate += OnValidate;
		}

		static void OnValidate(UnityEngine.Object instance)
		{
			ResetPool(instance);
			GameObject gameObject = null;

			if (instance is GameObject)
				gameObject = (GameObject)instance;
			else if (instance is Component)
				gameObject = ((Component)instance).gameObject;

			if (gameObject == null)
				return;

			var components = gameObject.GetComponents<Component>();

			for (int i = 0; i < components.Length; i++)
				ResetPool(components[i]);
		}
#endif
	}
}