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
	public class InjectableAutoProperty : InjectableMemberBase<PropertyInfo>, IInjectableProperty
	{
		readonly FieldInfo backingField;

		public InjectableAutoProperty(PropertyInfo property) : base(property)
		{
			backingField = property.GetBackingField();
		}

		protected override void SetupContext(ref InjectionContext context)
		{
			base.SetupContext(ref context);

			context.Type = ContextTypes.AutoProperty;
			context.ContractType = member.PropertyType;
		}

		protected override bool CanInject(ref InjectionContext context)
		{
			return context.Container.Resolver.CanResolve(context);
		}

		protected override object Inject(ref InjectionContext context)
		{
			var value = context.Container.Resolver.Resolve(context);
			backingField.SetValue(context.Instance, value);

			return value;
		}
	}
}
