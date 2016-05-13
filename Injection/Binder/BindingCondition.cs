using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using UnityEngine.Assertions;

namespace Pseudo.Injection.Internal
{
	public class BindingCondition : IBindingCondition
	{
		public IBinding Binding { get; private set; }

		public BindingCondition(IBinding binding)
		{
			Binding = binding;
		}

		public IBinding When(Predicate<InjectionContext> condition)
		{
			Binding.Condition = condition;

			return Binding;
		}

		public IBinding When(ConditionSource source, ConditionComparer comparer, object target)
		{
			return When(ToCondition(source, comparer, target));
		}

		static Predicate<InjectionContext> ToCondition(ConditionSource source, ConditionComparer comparer, object target)
		{
			return c =>
			{
				switch (source)
				{
					default:
					case ConditionSource.Container:
						return ConditionComparer(c.Container, (IContainer)target, comparer);
					case ConditionSource.Element:
						return ConditionComparer(c.Element, (IInjectableElement)target, comparer);
					case ConditionSource.ContextType:
						return ConditionComparer(c.Type, (ContextTypes)target, comparer);
					case ConditionSource.Instance:
						return ConditionComparer(c.Instance, target, comparer);
					case ConditionSource.ContractType:
						return ConditionComparer(c.ContractType, (Type)target, comparer);
					case ConditionSource.DeclaringType:
						return ConditionComparer(c.DeclaringType, (Type)target, comparer);
					case ConditionSource.Identifier:
						return ConditionComparer(c.Identifier, target, comparer);
					case ConditionSource.Optional:
						return ConditionComparer(c.Optional, (bool)target, comparer);
				}
			};
		}

		static bool ConditionComparer<T>(T source, T target, ConditionComparer comparer)
		{
			switch (comparer)
			{
				default:
				case Injection.ConditionComparer.Equals:
					return PEqualityComparer<T>.Default.Equals(source, target);
				case Injection.ConditionComparer.NotEquals:
					return !PEqualityComparer<T>.Default.Equals(source, target);
			}
		}
	}
}