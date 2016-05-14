using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Pooling.Internal;

namespace Pseudo.Pooling
{
	public class MultiPool<TBase> : MultiPool, IMultiPool<TBase> where TBase : class
	{
		public MultiPool(params IPool[] pools) : base(typeof(TBase), pools) { }

		public MultiPool(IPoolFactory factory) : base(typeof(TBase), factory) { }

		public MultiPool(Func<Type, IPool> factory) : base(typeof(TBase), factory) { }

		public T Create<T>() where T : class, TBase
		{
			return (T)base.Create(typeof(T));
		}

		new public TBase Create(Type type)
		{
			return (TBase)base.Create(type);
		}

		public bool Recycle<T>(T instance) where T : class, TBase
		{
			return base.Recycle(instance);
		}

		public bool Recycle(TBase instance)
		{
			return base.Recycle(instance);
		}

		public bool AddPool<T>(IPool<T> pool) where T : class, TBase
		{
			return base.AddPool(pool);
		}

		public bool RemovePool<T>() where T : class, TBase
		{
			return base.RemovePool(typeof(T));
		}

		public bool ContainsPool<T>() where T : class, TBase
		{
			return base.ContainsPool(typeof(T));
		}

		public IPool<T> GetPool<T>() where T : class, TBase
		{
			return base.GetPool(typeof(T)) as IPool<T>;
		}
	}

	public class MultiPool : IMultiPool
	{
		public IPoolFactory Factory
		{
			get { return factory; }
		}
		public Type BaseType
		{
			get { return baseType; }
		}
		public int PoolCount
		{
			get { return typeToPool.Count; }
		}

		readonly IPoolFactory factory;
		readonly Type baseType;
		readonly Dictionary<Type, IPool> typeToPool = new Dictionary<Type, IPool>();

		public MultiPool(Type baseType, IPoolFactory factory = null)
		{
			this.baseType = baseType;
			this.factory = factory ?? new DefaultPoolFactory();
		}

		public MultiPool(Type baseType, Func<Type, IPool> factory)
			: this(baseType, factory == null ? null : new PoolMethodFactory(factory)) { }

		public MultiPool(Type baseType, params IPool[] pools)
			: this(baseType, default(IPoolFactory))
		{
			for (int i = 0; i < pools.Length; i++)
				AddPool(pools[i]);
		}

		public object Create(Type type)
		{
			return GetPool(type).Create();
		}

		public bool Recycle(object instance)
		{
			if (instance == null)
				return false;
			else
				return GetPool(instance.GetType()).Recycle(instance);
		}

		public bool AddPool(IPool pool)
		{
			if (pool != null && pool.Type.Is(baseType))
			{
				typeToPool[pool.Type] = pool;
				return true;
			}
			else
				return false;
		}

		public bool RemovePool(Type type)
		{
			return typeToPool.Remove(type);
		}

		public bool ContainsPool(Type type)
		{
			return typeToPool.ContainsKey(type);
		}

		public IPool GetPool(Type type)
		{
			IPool pool;

			if (typeToPool.TryGetValue(type, out pool))
				return pool;
			else
			{
				pool = factory.Create(type);

				if (AddPool(pool))
					return pool;
				else
					throw new ArgumentException(string.Format("Can not find pool for type {0}.", type.Name));
			}
		}

		public void ClearPools()
		{
			typeToPool.Clear();
		}
	}
}
