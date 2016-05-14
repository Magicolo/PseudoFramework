using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;
using Pseudo.Reflection;

namespace Pseudo.Injection.Internal
{
	public class InjectableProperty : InjectableMemberBase<PropertyInfo>, IInjectableProperty
	{
		readonly IPropertyWrapper wrapper;

		public InjectableProperty(PropertyInfo property) : base(property)
		{
			wrapper = property.CreateWrapper();
		}

		protected override void SetupContext(ref InjectionContext context)
		{
			base.SetupContext(ref context);

			context.Type = ContextTypes.Property;
			context.ContractType = provider.PropertyType;
		}

		protected override object Inject(ref InjectionContext context)
		{
			var value = context.Container.Resolver.Resolve(context);
			wrapper.Set(ref context.Instance, value);

			return value;
		}
	}
}
