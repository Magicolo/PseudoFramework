using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using System.Reflection;
using System.Threading;
using System;
using Pseudo.Internal;
using Pseudo.Pooling.Internal;

namespace Pseudo.Pooling
{
	public static class TypePoolManager
	{
		static readonly Dictionary<Type, IPool> pools = new Dictionary<Type, IPool>(8);

		public static int StartSize = 2;

		public static T Create<T>() where T : class
		{
			var pool = GetPool<T>();
			var instance = pool.Create();

			return (T)instance;
		}

		public static object Create(Type type)
		{
			var pool = GetPool(type);
			var instance = pool.Create();

			return instance;
		}

		public static void Recycle(object instance)
		{
			if (instance == null)
				return;

			var pool = GetPool(instance.GetType());
			pool.Recycle(instance);
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

		public static IPool GetPool<T>() where T : class
		{
			return PoolHolder<T>.Pool;
		}

		public static IPool GetPool(Type type)
		{
			IPool pool;

			if (!pools.TryGetValue(type, out pool))
			{
				pool = PoolUtility.CreateTypePool(type, StartSize);
				pools[type] = pool;
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
		}
	}
}