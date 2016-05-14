using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Pooling
{
	public interface IMultiPool<TBase> : IMultiPool where TBase : class
	{
		T Create<T>() where T : class, TBase;
		new TBase Create(Type type);
		bool Recycle<T>(T instance) where T : class, TBase;
		bool Recycle(TBase instance);
		bool AddPool<T>(IPool<T> pool) where T : class, TBase;
		bool RemovePool<T>() where T : class, TBase;
		bool ContainsPool<T>() where T : class, TBase;
		IPool<T> GetPool<T>() where T : class, TBase;
	}

	public interface IMultiPool
	{
		IPoolFactory Factory { get; }
		Type BaseType { get; }
		int PoolCount { get; }

		object Create(Type type);
		bool Recycle(object instance);
		bool AddPool(IPool pool);
		bool RemovePool(Type type);
		bool ContainsPool(Type type);
		IPool GetPool(Type type);
		void ClearPools();
	}
}
