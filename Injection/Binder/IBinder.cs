using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;

namespace Pseudo.Injection
{
	public enum BindingType
	{
		Singleton,
		Transient,
		Factory
	}

	public interface IBinder
	{
		IContainer Container { get; }

		void Bind(IBinding binding);
		void Bind(Assembly assembly);
		IBindingContext Bind(Type contractType);
		IBindingContext Bind(Type contractType, params Type[] baseTypes);
		IBindingContext<TContract> Bind<TContract>();
		IBindingContext<TContract> Bind<TContract, TBase>() where TContract : TBase;
		IBindingContext<TContract> Bind<TContract, TBase1, TBase2>() where TContract : TBase1, TBase2;
		IBindingContext<TContract> Bind<TContract, TBase1, TBase2, TBase3>() where TContract : TBase1, TBase2, TBase3;
		IBindingContext<TContract> Bind<TContract>(params Type[] baseTypes);
		IBindingContext BindAll(Type contractType);
		IBindingContext<TContract> BindAll<TContract>();

		void Unbind(IBinding binding);
		void Unbind(Type contractType);
		void Unbind(params Type[] contractTypes);
		void UnbindAll(Type contractType);
		void Unbind<TContract>();
		void UnbindAll<TContract>();
		void UnbindAll();

		bool HasBinding(IBinding binding);
		bool HasBinding(Type contractType);
		bool HasBinding<TContract>();

		IBinding GetBinding(InjectionContext context);
		IBinding GetBinding(Type contractType);
		IBinding GetBinding<TContract>();
		IEnumerable<IBinding> GetBindings(InjectionContext context);
		IEnumerable<IBinding> GetBindings(Type contractType);
		IEnumerable<IBinding> GetBindings<TContract>();
		IEnumerable<IBinding> GetAllBindings();
	}
}
