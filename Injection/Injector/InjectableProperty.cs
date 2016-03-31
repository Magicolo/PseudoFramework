using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;

namespace Pseudo.Injection.Internal
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

			if (!attribute.Optional || context.Binder.Resolver.CanResolve(context))
				property.SetValue(context.Instance, context.Binder.Resolver.Resolve(context), null);
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
