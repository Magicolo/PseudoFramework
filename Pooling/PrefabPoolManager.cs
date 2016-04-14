using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal;
using Pseudo.Pooling.Internal;

namespace Pseudo.Pooling
{
	public static class PrefabPoolManager
	{
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

		public static void RecycleElements(IList elements)
		{
			if (elements == null)
				return;

			for (int i = 0; i < elements.Count; i++)
				Recycle(elements[i]);

			elements.Clear();
		}

		public static IPool GetPool(object prefab)
		{
			if (prefab == null)
				return null;

			IPool pool;

			if (!pools.TryGetValue(prefab, out pool))
			{
				pool = PoolUtility.CreatePrefabPool(prefab);
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
			if (prefab == null)
				return;

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
			if (prefab == null)
				return;

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
			Pseudo.Editor.Internal.InspectorUtility.OnValidate += OnValidate;
		}

		static void OnValidate(UnityEngine.Object instance)
		{
			if (!ApplicationUtility.IsPlaying || UnityEditor.PrefabUtility.GetPrefabType(instance) != UnityEditor.PrefabType.Prefab)
				return;

			Transform root = null;

			if (instance is GameObject)
				root = ((GameObject)instance).transform.root;
			else if (instance is Component)
				root = ((Component)instance).transform.root;

			if (root == null)
				return;

			ResetPool(root.gameObject);
			var components = root.GetComponents<Component>();

			for (int i = 0; i < components.Length; i++)
				ResetPool(components[i]);
		}
#endif
	}
}