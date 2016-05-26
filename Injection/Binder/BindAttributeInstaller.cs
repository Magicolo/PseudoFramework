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
		readonly BindAttributeBase attribute;
		readonly Type concreteType;

		public BindAttributeInstaller(BindAttributeBase attribute, Type concreteType)
		{
			this.attribute = attribute;
			this.concreteType = concreteType;
		}

		public void Install(IContainer container)
		{
			attribute.Install(container, concreteType);
		}

		public override string ToString()
		{
			return string.Format("{0}({1})", GetType().Name, concreteType.FullName);
		}
	}
}