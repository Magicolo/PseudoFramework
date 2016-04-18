using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Injection.Internal;

namespace Pseudo.Injection
{
	public sealed class BindAllAttribute : BindAttributeBase
	{
		public BindAllAttribute(BindScope bindingScope) : base(bindingScope) { }

		public BindAllAttribute(BindScope bindingScope, ConditionSource conditionSource, ConditionComparer conditionComparer, object conditionTarget) : base(bindingScope, conditionSource, conditionComparer, conditionTarget) { }

		protected override IBindingContract Bind(IContainer container, Type concreteType)
		{
			return container.Binder.BindAll(concreteType);
		}
	}
}
