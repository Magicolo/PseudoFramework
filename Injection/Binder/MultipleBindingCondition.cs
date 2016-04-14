using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Injection.Internal
{
	public class MultipleBindingCondition : BindingConditionBase
	{
		readonly IBindingCondition[] conditions;

		public MultipleBindingCondition(IBindingCondition[] conditions)
		{
			this.conditions = conditions;
		}

		public override void When(Predicate<InjectionContext> condition)
		{
			for (int i = 0; i < conditions.Length; i++)
				conditions[i].When(condition);
		}
	}
}
