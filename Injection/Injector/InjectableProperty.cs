using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;
using Pseudo.Internal;

namespace Pseudo.Injection.Internal
{
	public class InjectableProperty : InjectableMemberBase<PropertyInfo>, IInjectableProperty
	{
		public PropertyInfo Property
		{
			get { return member; }
		}

		public InjectableProperty(PropertyInfo property) : base(property) { }

		public override bool CanInject(InjectionContext context)
		{
			return context.Container.Resolver.CanResolve(context);
		}

		protected override void SetupContext(ref InjectionContext context)
		{
			context.ContextType = InjectionContext.ContextTypes.Property;
			context.ContractType = member.PropertyType;
			context.DeclaringType = member.DeclaringType;
			context.Identifier = attribute.Identifier;
			context.Optional = attribute.Optional;
		}

		protected override object Inject(ref InjectionContext context)
		{
			var value = context.Container.Resolver.Resolve(context);
			member.SetValue(context.Instance, value, null);

			return value;
		}
	}
}
