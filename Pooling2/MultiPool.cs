using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Pooling2
{
	public class MultiPool<TBase> : IMultiPool<TBase> where TBase : class
	{
		readonly Dictionary<Type, IPool> typeToPool = new Dictionary<Type, IPool>();

		public MultiPool(params IPool[] pools)
		{
			for (int i = 0; i < pools.Length; i++)
				AddPool(pools[i]);
		}

		public T Create<T>() where T : class, TBase
		{
			return GetPool<T>().Create();
		}

		public TBase Create(Type type)
		{
			return (TBase)GetPool(type).Create();
		}

		public bool Recycle<T>(T instance) where T : class, TBase
		{
			return GetPool<T>().Recycle(instance);
		}

		public bool Recycle(object instance)
		{
			if (instance is TBase)
				return GetPool(instance.GetType()).Recycle(instance);
			else
				return false;
		}

		public void AddPool<T>(IPool<T> pool) where T : class, TBase
		{
			AddPool((IPool)pool);
		}

		public void AddPool(IPool pool)
		{
			if (pool != null && pool.Type.Is<TBase>())
				typeToPool[pool.Type] = pool;
		}

		public bool RemovePool<T>() where T : class, TBase
		{
			return RemovePool(typeof(T));
		}

		public bool RemovePool(Type type)
		{
			return typeToPool.Remove(type);
		}

		public bool ContainsPool<T>() where T : class, TBase
		{
			return ContainsPool(typeof(T));
		}

		public bool ContainsPool(Type type)
		{
			return typeToPool.ContainsKey(type);
		}

		public IPool<T> GetPool<T>() where T : class, TBase
		{
			return (IPool<T>)GetPool(typeof(T));
		}

		public IPool GetPool(Type type)
		{
			IPool pool;

			if (typeToPool.TryGetValue(type, out pool))
				return pool;
			else
				throw new ArgumentException(string.Format("Can not find pool for type {0}.", type.Name));
		}

		public void ClearPools()
		{
			typeToPool.Clear();
		}

		object IMultiPool.Create(Type type)
		{
			return Create(type);
		}
	}
}
