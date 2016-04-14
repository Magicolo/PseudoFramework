using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using UnityEngine.Assertions;

namespace Pseudo.Injection.Internal
{
	public class BindingCondition : BindingConditionBase
	{
		readonly IBinding binding;

		public BindingCondition(IBinding binding)
		{
			this.binding = binding;
		}

		public override void When(Predicate<InjectionContext> condition)
		{
			binding.Condition = condition;
		}
	}
}
