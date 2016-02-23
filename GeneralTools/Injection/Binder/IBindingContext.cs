using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo
{
	public delegate TResult InjectionMethod<out TResult>(InjectionContext context);

	public interface IBindingContext
	{
		IBindingCondition ToSingle();
		IBindingCondition ToSingle(Type concreteType);
		IBindingCondition ToSinglePrefab(UnityEngine.Object prefab);
		IBindingCondition ToSinglePrefab(GameObject prefab);
		IBindingCondition ToSingleMethod(InjectionMethod<object> method);
		IBindingCondition ToTransient();
		IBindingCondition ToTransient(Type concreteType);
		IBindingCondition ToTransientPrefab(UnityEngine.Object prefab);
		IBindingCondition ToTransientPrefab(GameObject prefab);
		IBindingCondition ToTransientMethod(InjectionMethod<object> method);
		IBindingCondition ToInstance(object instance);
		IBindingCondition ToFactory(Type factoryType);
		IBindingCondition ToFactory(IFactory factory);
		IBindingCondition ToFactory(IInjectionFactory factory);
	}

	public interface IBindingContext<TContract> : IBindingContext where TContract : class
	{
		IBindingCondition ToSingle<TConcrete>() where TConcrete : class, TContract;
		IBindingCondition ToSinglePrefab<TConcrete>(TConcrete prefab) where TConcrete : UnityEngine.Object, TContract;
		IBindingCondition ToSingleMethod<TConcrete>(InjectionMethod<TConcrete> method) where TConcrete : class, TContract;
		IBindingCondition ToTransient<TConcrete>() where TConcrete : class, TContract;
		IBindingCondition ToTransientPrefab<TConcrete>(TConcrete prefab) where TConcrete : UnityEngine.Object, TContract;
		IBindingCondition ToTransientMethod<TConcrete>(InjectionMethod<TConcrete> method) where TConcrete : class, TContract;
		IBindingCondition ToInstance<TConcrete>(TConcrete instance) where TConcrete : class, TContract;
		IBindingCondition ToFactory<TFactory>() where TFactory : class, IFactory<TContract>;
		IBindingCondition ToFactory(IFactory<TContract> factory);
	}
}
