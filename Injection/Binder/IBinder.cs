using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;

namespace Pseudo.Injection
{
	public interface IBinder
	{
		IContainer Container { get; }
		IBindingSelector BindingSelector { get; set; }

		void Bind(IBinding binding);
		void Bind(Assembly assembly);
		IBindingContract Bind(Type contractType, params Type[] baseTypes);
		IBindingContract<TContract> Bind<TContract>(params Type[] baseTypes);
		IBindingContract BindAll(Type contractType);
		IBindingContract<TContract> BindAll<TContract>();

		void Unbind(IBinding binding);
		void Unbind(Type contractType);
		void Unbind(params Type[] contractTypes);
		void UnbindAll(Type contractType);
		void UnbindAll();

		bool HasBinding(IBinding binding);
		bool HasBinding(InjectionContext context);
		IBinding GetBinding(InjectionContext context);
		IEnumerable<IBinding> GetBindings(InjectionContext context);
		IEnumerable<IBinding> GetAllBindings();
	}
}
