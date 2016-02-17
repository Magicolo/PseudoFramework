using UnityEngine;
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

		public void WhenIs(string identifier)
		{
			for (int i = 0; i < conditions.Length; i++)
				conditions[i].WhenIs(identifier);
		}

		public void WhenIs(InjectionContext.Types contextType)
		{
			for (int i = 0; i < conditions.Length; i++)
				conditions[i].WhenIs(contextType);
		}

		public void WhenInjectedInto(Type declaringType)
		{
			for (int i = 0; i < conditions.Length; i++)
				conditions[i].WhenInjectedInto(declaringType);
		}

		public void WhenInjectedInto(object instance)
		{
			for (int i = 0; i < conditions.Length; i++)
				conditions[i].WhenInjectedInto(instance);
		}
	}
}
