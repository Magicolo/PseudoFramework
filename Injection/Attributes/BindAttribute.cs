using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using UnityEngine.Scripting;
using Pseudo.Injection.Internal;
using UnityEngine.Assertions;

namespace Pseudo.Injection
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Delegate, AllowMultiple = true, Inherited = true)]
	public sealed class BindAttribute : PreserveAttribute
	{
		public readonly Type ContractType;
		public readonly Type[] BaseTypes;
		public readonly BindingType BindingType;
		public readonly Predicate<InjectionContext> Condition;

		public BindAttribute(Type contractType, BindingType bindingType)
			: this(contractType, Type.EmptyTypes, bindingType, null)
		{ }

		public BindAttribute(Type contractType, Type[] baseTypes, BindingType bindingType)
			: this(contractType, baseTypes, bindingType, null)
		{ }

		public BindAttribute(Type contractType, BindingType bindingType, ConditionSource conditionSource, ConditionComparer conditionComparer, object conditionTarget)
			: this(contractType, Type.EmptyTypes, bindingType, InjectionUtility.GetCondition(conditionSource, conditionComparer, conditionTarget))
		{ }

		public BindAttribute(Type contractType, Type[] baseTypes, BindingType bindingType, ConditionSource conditionSource, ConditionComparer conditionComparer, object conditionTarget)
			: this(contractType, baseTypes, bindingType, InjectionUtility.GetCondition(conditionSource, conditionComparer, conditionTarget))
		{ }

		BindAttribute(Type contractType, Type[] baseTypes, BindingType bindingType, Predicate<InjectionContext> condition)
		{
			ContractType = contractType;
			BaseTypes = baseTypes;
			BindingType = bindingType;
			Condition = condition;
		}
	}
}
