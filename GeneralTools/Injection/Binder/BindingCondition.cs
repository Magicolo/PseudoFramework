using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using UnityEngine.Assertions;

namespace Pseudo.Internal.Injection
{
	public class BindingCondition : IBindingCondition
	{
		readonly FactoryData data;

		public BindingCondition(FactoryData data)
		{
			this.data = data;
		}

		public void When(Predicate<InjectionContext> condition)
		{
			Assert.IsNotNull(condition);

			data.Condition = condition;
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
