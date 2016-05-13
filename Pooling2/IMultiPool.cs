using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Pooling2
{
	public interface IMultiPool<TBase> : IMultiPool where TBase : class
	{
		T Create<T>() where T : class, TBase;
		new TBase Create(Type type);
		bool Recycle<T>(T instance) where T : class, TBase;
		void AddPool<T>(IPool<T> pool) where T : class, TBase;
		bool RemovePool<T>() where T : class, TBase;
		bool ContainsPool<T>() where T : class, TBase;
		IPool<T> GetPool<T>() where T : class, TBase;
	}

	public interface IMultiPool
	{
		object Create(Type type);
		bool Recycle(object instance);
		void AddPool(IPool pool);
		bool RemovePool(Type type);
		bool ContainsPool(Type type);
		IPool GetPool(Type type);
		void ClearPools();
	}
}
