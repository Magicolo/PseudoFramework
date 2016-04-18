using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Injection.Internal
{
	public class BindingScope : IBindingScope
	{
		public IBinding Binding { get; private set; }

		public BindingScope(IBinding binding)
		{
			Binding = binding;
		}

		public IBindingCondition AsSingleton()
		{
			return As(new SingletonScope());
		}

		public IBindingCondition AsTransient()
		{
			return As(new TransientScope());
		}

		public IBindingCondition As(BindScope scope)
		{
			switch (scope)
			{
				default:
				case Injection.BindScope.Transient:
					return AsTransient();
				case Injection.BindScope.Singleton:
					return AsSingleton();
			}
		}

		public IBindingCondition As(IInjectionScope scope)
		{
			Binding.Scope = scope;

			return new BindingCondition(Binding);
		}
	}
}
