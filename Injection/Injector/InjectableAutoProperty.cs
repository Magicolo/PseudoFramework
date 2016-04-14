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
		public PropertyInfo Property
		{
			get { return member; }
		}

		readonly FieldInfo backingField;

		public InjectableAutoProperty(PropertyInfo property) : base(property)
		{
			backingField = property.GetBackingField();
		}

		public override bool CanInject(InjectionContext context)
		{
			return context.Container.Resolver.CanResolve(context);
		}

		protected override void SetupContext(ref InjectionContext context)
		{
			context.ContextType = InjectionContext.ContextTypes.AutoProperty;
			context.ContractType = member.PropertyType;
			context.DeclaringType = member.DeclaringType;
			context.Identifier = attribute.Identifier;
			context.Optional = attribute.Optional;
		}

		protected override object Inject(ref InjectionContext context)
		{
			var value = context.Container.Resolver.Resolve(context);
			backingField.SetValue(context.Instance, value);

			return value;
		}
	}
}
