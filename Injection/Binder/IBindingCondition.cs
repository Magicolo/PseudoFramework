using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Injection
{
	public enum ConditionSource
	{
		Container,
		Element,
		ContextType,
		Instance,
		ContractType,
		DeclaringType,
		Identifier,
		Optional,
	}

	public enum ConditionComparer
	{
		Equals,
		NotEquals
	}

	public interface IBindingCondition
	{
		IBinding Binding { get; }

		IBinding When(Predicate<InjectionContext> condition);
		IBinding When(ConditionSource conditionSource, ConditionComparer conditionComparer, object conditionTarget);
	}
}
