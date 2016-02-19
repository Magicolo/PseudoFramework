using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;

namespace Pseudo.Internal.Injection
{
	public class InjectableProperty : IInjectableMember
	{
		public MemberInfo Member
		{
			get { return property; }
		}

		readonly PropertyInfo property;
		readonly InjectAttribute attribute;

		public InjectableProperty(PropertyInfo property)
		{
			this.property = property;

			attribute = (InjectAttribute)property.GetCustomAttributes(typeof(InjectAttribute), true).First() ?? new InjectAttribute();
		}

		public void Inject(InjectionContext context)
		{
			SetupContext(ref context);

			if (!context.Optional || context.Binder.Resolver.CanResolve(context))
				property.SetValue(context.Instance, context.Binder.Resolver.Resolve(context), null);
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
