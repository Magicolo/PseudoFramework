using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal.Injection
{
	public class MultipleBindingCondition : IBindingCondition
	{
		readonly IBindingCondition[] conditions;

		public MultipleBindingCondition(IBindingCondition[] conditions)
		{
			this.conditions = conditions;
		}

		public void When(Predicate<InjectionContext> condition)
		{
			for (int i = 0; i < conditions.Length; i++)
				conditions[i].When(condition);
		}

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
	}
}
