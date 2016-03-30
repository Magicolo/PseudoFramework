using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;

namespace Pseudo.Internal.Injection
{
	public class InjectableAutoProperty : IInjectableMember
	{
		public MemberInfo Member
		{
			get { return property; }
		}

		readonly PropertyInfo property;
		readonly FieldInfo backingField;
		readonly InjectAttribute attribute;

		public InjectableAutoProperty(PropertyInfo property)
		{
			this.property = property;

			backingField = property.GetBackingField();
			attribute = (InjectAttribute)property.GetCustomAttributes(typeof(InjectAttribute), true).First() ?? new InjectAttribute();
		}

		public void Inject(InjectionContext context)
		{
			SetupContext(ref context);

			if (!attribute.Optional || context.Binder.Resolver.CanResolve(context))
				backingField.SetValue(context.Instance, context.Binder.Resolver.Resolve(context));
		}

		void SetupContext(ref InjectionContext context)
		{
			context.ContextType = InjectionContext.ContextTypes.Property;
			context.ContractType = property.PropertyType;
			context.Member = property;
			context.Attribute = attribute;
		}
	}
}
