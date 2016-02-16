using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

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
			data.Conditions.Add(condition);
		}

		public void WhenIdentifierIs(string identifier)
		{
			When(context => context.Identifier == identifier);
		}

		public void WhenInjectedInto(Type declaringType)
		{
			When(context => context.DeclaringType == declaringType);
		}

		public void WhenInjectionInto(InjectionContext.Types contextType)
		{
			When(context => context.Type == contextType);
		}

		public void WhenInjectionInto(object instance)
		{
			When(context => context.Instance == instance);
		}
	}
}
