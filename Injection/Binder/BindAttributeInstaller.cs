using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Injection.Internal
{
	public class BindAttributeInstaller : IBindingInstaller
	{
		readonly BindAttribute attribute;
		readonly Type concreteType;

		public BindAttributeInstaller(BindAttribute attribute, Type concreteType)
		{
			this.attribute = attribute;
			this.concreteType = concreteType;
		}

		public void Install(IContainer container)
		{
			var context = container.Binder.Bind(attribute.ContractType, attribute.BaseTypes);
			IBindingCondition bindingCondition;

			switch (attribute.BindingType)
			{
				default:
				case BindingType.Singleton:
					bindingCondition = context.ToSingleton(concreteType);
					break;
				case BindingType.Transient:
					bindingCondition = context.ToTransient(concreteType);
					break;
				case BindingType.Factory:
					bindingCondition = context.ToFactory(concreteType);
					break;
			}

			bindingCondition.When(attribute.Condition);
		}

		public override string ToString()
		{
			return string.Format("{0}({1})", GetType().Name, concreteType.FullName);
		}
	}
}