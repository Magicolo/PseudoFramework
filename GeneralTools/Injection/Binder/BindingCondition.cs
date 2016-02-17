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
		readonly Type contractType;
		readonly FactoryData data;
		readonly Resolver resolver;

		public BindingCondition(Type contractType, FactoryData data, Resolver resolver)
		{
			this.contractType = contractType;
			this.data = data;
			this.resolver = resolver;
		}

		public void When(Predicate<InjectionContext> condition)
		{
			data.Conditions.Add(condition);
			resolver.Sort(contractType);
		}

		public void WhenIs(string identifier)
		{
			When(context => context.Identifier == identifier);
		}

		public void WhenIs(InjectionContext.Types contextType)
		{
			When(context => context.Type == contextType);
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
