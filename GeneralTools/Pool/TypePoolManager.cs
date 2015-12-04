using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using System.Reflection;
using System.Threading;
using System;
using Pseudo.Internal;
using Pseudo.Internal.Pool;

namespace Pseudo
{
	public static class TypePoolManager
	{
		static readonly Dictionary<Type, Pool> pools = new Dictionary<Type, Pool>(16);

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

		public static ICopyable CreateCopy(ICopyable reference)
		{
			if (reference == null)
				return null;

			var pool = GetPool(reference.GetType());
			var instance = pool.CreateCopy(reference);

			return instance;
		}

		public static void CreateCopies<T>(ref T[] targets, T[] sources) where T : class, ICopyable
		{
			if (sources == null)
				return;

			if (targets == null)
				targets = new T[sources.Length];
			else if (targets.Length != sources.Length)
				Array.Resize(ref targets, sources.Length);

			for (int i = 0; i < targets.Length; i++)
				targets[i] = (T)CreateCopy(sources[i]);
		}

		public static void CreateCopies<T>(List<T> targets, IList<T> sources) where T : class, ICopyable
		{
			if (sources == null)
				return;

			if (targets == null)
				targets = new List<T>(sources.Count);
			else
				targets.Clear();

			for (int i = 0; i < targets.Count; i++)
				targets.Add((T)CreateCopy(sources[i]));
		}

		public static void CreateElements<T>(IList<T> elements) where T : class, ICopyable
		{
			if (elements == null)
				return;

			for (int i = 0; i < elements.Count; i++)
				elements[i] = (T)CreateCopy(elements[i]);
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

		public static Pool GetPool<T>() where T : class
		{
			return PoolHolder<T>.Pool;
		}

		public static Pool GetPool(Type type)
		{
			Pool pool;

			if (!pools.TryGetValue(type, out pool))
			{
				pool = PoolUtility.CreatePool(Activator.CreateInstance(type), StartSize);
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