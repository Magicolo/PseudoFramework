using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Injection
{
	public enum BindScope
	{
		Singleton,
		Transient,
	}

	public interface IBindingScope
	{
		IBinding Binding { get; }

		IBindingCondition AsSingleton();
		IBindingCondition AsTransient();
		IBindingCondition As(BindScope scope);
		IBindingCondition As(IInjectionScope scope);
	}

	public interface IBindingScope<TConcrete> : IBindingScope
	{
		IBindingCondition As(IInjectionScope<TConcrete> scope);
	}
}
