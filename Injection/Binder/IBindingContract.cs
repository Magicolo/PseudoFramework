using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Injection
{
	public delegate TResult InjectionMethod<out TResult>(InjectionContext context);

	public interface IBindingContract
	{
		Type ContractType { get; }
		Type[] BaseTypes { get; }
		IContainer Container { get; }

		IBindingScope ToSelf();
		IBindingScope To(Type concreteType);
		IBindingCondition ToInstance(object instance);
		IBindingScope ToMethod(InjectionMethod<object> method);
		IBindingScope ToFactory(Type factoryType);
		IBindingScope ToFactory(IInjectionFactory factory);
	}

	public interface IBindingContract<TContract> : IBindingContract
	{
		IBindingScope To<TConcrete>() where TConcrete : TContract;
		IBindingCondition ToInstance<TConcrete>(TConcrete instance) where TConcrete : TContract;
		IBindingScope ToMethod<TConcrete>(InjectionMethod<TConcrete> method) where TConcrete : TContract;
		IBindingScope ToFactory<TFactory>() where TFactory : IInjectionFactory<TContract>;
		IBindingScope ToFactory<TConcrete>(IInjectionFactory<TConcrete> factory) where TConcrete : TContract;
	}
}
