using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo
{
	public interface IBinder
	{
		IBinder Parent { get; }
		IResolver Resolver { get; }
		IInjector Injector { get; }
		IInstantiator Instantiator { get; }

		IBindingContext Bind(Type contractType);
		IBindingContext Bind(Type contractType, params Type[] baseTypes);
		IBindingContext<TContract> Bind<TContract>() where TContract : class;
		IBindingContext<TContract> Bind<TContract, TBase>() where TContract : class, TBase;
		IBindingContext<TContract> Bind<TContract, TBase1, TBase2>() where TContract : class, TBase1, TBase2;
		IBindingContext<TContract> Bind<TContract, TBase1, TBase2, TBase3>() where TContract : class, TBase1, TBase2, TBase3;
		IBindingContext<TContract> Bind<TContract>(params Type[] baseTypes) where TContract : class;
		IBindingContext BindAll(Type contractType);
		IBindingContext<TContract> BindAll<TContract>() where TContract : class;
		void Unbind(Type contractType);
		void Unbind(params Type[] contractTypes);
		void UnbindAll(Type contractType);
		void Unbind<TContract>();
		void UnbindAll<TContract>();
		void UnbindAll();
		bool HasBinding(Type contractType);
		bool HasBinding<TContract>();
		void Install(IBindingInstaller installer);
	}
}
