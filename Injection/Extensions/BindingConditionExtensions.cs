using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Injection
{
	public static class BindingConditionExtensions
	{
		public static IBinding WhenInto(this IBindingCondition condition, ContextTypes type)
		{
			return condition.When(context => context.Type.Contains(type));
		}

		public static IBinding WhenInto(this IBindingCondition condition, Type declaringType)
		{
			return condition.When(context => context.DeclaringType == declaringType);
		}

		public static IBinding WhenInto(this IBindingCondition condition, object instance)
		{
			return condition.When(context => context.Instance == instance);
		}

		public static IBinding WhenHas(this IBindingCondition condition, object identifier)
		{
			return condition.When(context => Equals(context.Identifier, identifier));
		}

		public static IBinding WhenHas(this IBindingCondition condition, bool optional)
		{
			return condition.When(context => context.Optional == optional);
		}

		public static IBinding WhenHas(this IBindingCondition condition, Type attributeType)
		{
			return condition.When(c => c.Element == null || c.Element.IsDefined(attributeType, true));
		}
	}
}
