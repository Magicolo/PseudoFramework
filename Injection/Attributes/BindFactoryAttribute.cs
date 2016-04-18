using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Injection.Internal;

namespace Pseudo.Injection
{
	public sealed class BindFactoryAttribute : BindAttributeBase
	{
		public readonly Type ContractType;
		public readonly Type[] BaseTypes;

		public BindFactoryAttribute(Type contractType, BindScope scope)
			: this(contractType, Type.EmptyTypes, scope) { }

		public BindFactoryAttribute(Type contractType, Type[] baseTypes, BindScope scope)
			: base(scope)
		{
			ContractType = contractType;
			BaseTypes = baseTypes;
		}

		public BindFactoryAttribute(Type contractType, BindScope scope, ConditionSource conditionSource, ConditionComparer conditionComparer, object conditionTarget)
			: this(contractType, Type.EmptyTypes, scope, conditionSource, conditionComparer, conditionTarget) { }

		public BindFactoryAttribute(Type contractType, Type[] baseTypes, BindScope scope, ConditionSource conditionSource, ConditionComparer conditionComparer, object conditionTarget)
			: base(scope, conditionSource, conditionComparer, conditionTarget)
		{
			ContractType = contractType;
			BaseTypes = baseTypes;
		}

		protected override IBindingContract Bind(IContainer container, Type concreteType)
		{
			return container.Binder.Bind(ContractType, BaseTypes);
		}

		protected override IBindingScope To(IBindingContract contract, Type concreteType)
		{
			return contract.ToFactory(concreteType);
		}
	}
}
