using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo
{
	public delegate TResult InjectionMethod<out TResult>(IBinder binder, params object[] arguments);

	public interface IBindingContext
	{
		IBindingCondition ToSingle();
		IBindingCondition ToSingle(Type concreteType);
		IBindingCondition ToSinglePrefab(UnityEngine.Object prefab);
		IBindingCondition ToSinglePrefab(GameObject prefab);
		IBindingCondition ToTransient();
		IBindingCondition ToTransient(Type concreteType);
		IBindingCondition ToTransientPrefab(UnityEngine.Object prefab);
		IBindingCondition ToTransientPrefab(GameObject prefab);
		IBindingCondition ToInstance(object instance);
		IBindingCondition ToMethod(InjectionMethod<object> method);
		IBindingCondition ToFactory(IFactory factory);
	}

	public interface IBindingContext<TContract> : IBindingContext
	{
		IBindingCondition ToSingle<TConcrete>() where TConcrete : class, TContract;
		IBindingCondition ToSinglePrefab<TConcrete>(TConcrete prefab) where TConcrete : UnityEngine.Object, TContract;
		IBindingCondition ToTransient<TConcrete>() where TConcrete : class, TContract;
		IBindingCondition ToTransientPrefab<TConcrete>(TConcrete prefab) where TConcrete : UnityEngine.Object, TContract;
		IBindingCondition ToInstance<TConcrete>(TConcrete instance) where TConcrete : class, TContract;
		IBindingCondition ToMethod<TConcrete>(InjectionMethod<TConcrete> method) where TConcrete : class, TContract;
		IBindingCondition ToFactory<TConcrete>(IFactory<TConcrete> factory) where TConcrete : class, TContract;
	}
}
