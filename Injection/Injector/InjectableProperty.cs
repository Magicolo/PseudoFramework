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
		public InjectableProperty(PropertyInfo property) : base(property) { }

		protected override void SetupContext(ref InjectionContext context)
		{
			base.SetupContext(ref context);

			context.Type = ContextTypes.Property;
			context.ContractType = member.PropertyType;
		}

		protected override bool CanInject(ref InjectionContext context)
		{
			return context.Container.Resolver.CanResolve(context);
		}

		protected override object Inject(ref InjectionContext context)
		{
			var value = context.Container.Resolver.Resolve(context);
			member.SetValue(context.Instance, value, null);

			return value;
		}
	}
}
