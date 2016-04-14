using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Injection.Internal
{
	public abstract class BindingConditionBase : IBindingCondition
	{
		public void When(string identifier)
		{
			When(context => context.Identifier == identifier);
		}

		public void When(InjectionContext.ContextTypes contextType)
		{
			When(context => (context.ContextType & contextType) != 0);
		}

		public void WhenInjectedInto(Type declaringType)
		{
			When(context => context.DeclaringType == declaringType);
		}

		public void WhenInjectedInto(object instance)
		{
			When(context => context.Instance == instance);
		}

		public abstract void When(Predicate<InjectionContext> condition);
	}
}
