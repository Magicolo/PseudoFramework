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
			get
			{
				throw new NotImplementedException();
			}
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

			if (!context.Optional || context.Binder.Resolver.CanResolve(context))
				backingField.SetValue(context.Instance, context.Binder.Resolver.Resolve(context));
		}

		void SetupContext(ref InjectionContext context)
		{
			context.ContextType = InjectionContext.ContextTypes.Property;
			context.ContractType = property.PropertyType;
			context.DeclaringType = property.DeclaringType;
			context.Member = property;
			context.Optional = attribute.Optional;
			context.Identifier = attribute.Identifier;
		}
	}
}
