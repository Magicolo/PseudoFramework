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
		IBindingCondition ToSingleton();
		IBindingCondition ToSingleton(Type concreteType);
		IBindingCondition ToSingletonPrefab(UnityEngine.Object prefab);
		IBindingCondition ToSingletonPrefab(GameObject prefab);
		IBindingCondition ToSingletonMethod(InjectionMethod<object> method);
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

	public interface IBindingContext<TContract> : IBindingContext
	{
		IBindingCondition ToSingleton<TConcrete>() where TConcrete : TContract;
		IBindingCondition ToSingletonPrefab<TConcrete>(TConcrete prefab) where TConcrete : UnityEngine.Object, TContract;
		IBindingCondition ToSingletonMethod<TConcrete>(InjectionMethod<TConcrete> method) where TConcrete : TContract;
		IBindingCondition ToTransient<TConcrete>() where TConcrete : TContract;
		IBindingCondition ToTransientPrefab<TConcrete>(TConcrete prefab) where TConcrete : UnityEngine.Object, TContract;
		IBindingCondition ToTransientMethod<TConcrete>(InjectionMethod<TConcrete> method) where TConcrete : TContract;
		IBindingCondition ToInstance<TConcrete>(TConcrete instance) where TConcrete : TContract;
		IBindingCondition ToFactory<TFactory>() where TFactory : IFactory<TContract>;
		IBindingCondition ToFactory(IFactory<TContract> factory);
	}
}
