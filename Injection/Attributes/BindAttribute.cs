using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Injection.Internal;

namespace Pseudo.Injection
{
	public sealed class BindAttribute : BindAttributeBase
	{
		public readonly Type ContractType;
		public readonly Type[] BaseTypes;

		public BindAttribute(Type contractType, BindScope bindingScope)
			: this(contractType, Type.EmptyTypes, bindingScope) { }

		public BindAttribute(Type contractType, Type[] baseTypes, BindScope bindingScope)
			: base(bindingScope)
		{
			ContractType = contractType;
			BaseTypes = baseTypes;
		}

		public BindAttribute(Type contractType, BindScope bindingScope, ConditionSource conditionSource, ConditionComparer conditionComparer, object conditionTarget)
			: this(contractType, Type.EmptyTypes, bindingScope, conditionSource, conditionComparer, conditionTarget) { }

		public BindAttribute(Type contractType, Type[] baseTypes, BindScope bindingScope, ConditionSource conditionSource, ConditionComparer conditionComparer, object conditionTarget)
			: base(bindingScope, conditionSource, conditionComparer, conditionTarget)
		{
			ContractType = contractType;
			BaseTypes = baseTypes;
		}

		protected override IBindingContract Bind(IContainer container, Type concreteType)
		{
			return container.Binder.Bind(ContractType, BaseTypes);
		}
	}
}
