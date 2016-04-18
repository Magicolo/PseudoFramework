using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using UnityEngine.Scripting;

namespace Pseudo.Injection.Internal
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Delegate, AllowMultiple = true, Inherited = true)]
	public abstract class BindAttributeBase : PreserveAttribute
	{
		public readonly BindScope BindingScope;
		public readonly ConditionSource ConditionSource = ConditionSource.Container;
		public readonly ConditionComparer ConditionComparer = ConditionComparer.NotEquals;
		public readonly object ConditionTarget;

		protected BindAttributeBase(BindScope bindingScope)
		{
			BindingScope = bindingScope;
		}

		protected BindAttributeBase(BindScope bindingScope, ConditionSource conditionSource, ConditionComparer conditionComparer, object conditionTarget)
			: this(bindingScope)
		{
			ConditionSource = conditionSource;
			ConditionComparer = conditionComparer;
			ConditionTarget = conditionTarget;
		}

		public IBinding Install(IContainer container, Type concreteType)
		{
			var contract = Bind(container, concreteType);
			var scope = To(contract, concreteType);
			var condition = As(scope, concreteType);
			var binding = When(condition, concreteType);

			return binding;
		}

		protected virtual IBindingScope To(IBindingContract contract, Type concreteType)
		{
			return contract.To(concreteType);
		}
		protected virtual IBindingCondition As(IBindingScope scope, Type concreteType)
		{
			return scope.As(BindingScope);
		}
		protected virtual IBinding When(IBindingCondition condition, Type concreteType)
		{
			return condition.When(ConditionSource, ConditionComparer, ConditionTarget);
		}

		protected abstract IBindingContract Bind(IContainer container, Type concreteType);
	}
}
