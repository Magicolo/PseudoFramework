using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;
using Pseudo.Reflection;

namespace Pseudo.Injection.Internal
{
	public class InjectableField : InjectableMemberBase<FieldInfo>, IInjectableField
	{
		readonly IFieldWrapper wrapper;

		public InjectableField(FieldInfo field) : base(field)
		{
			wrapper = field.CreateWrapper();
		}

		protected override void SetupContext(ref InjectionContext context)
		{
			base.SetupContext(ref context);

			context.Type = ContextTypes.Field;
			context.ContractType = provider.FieldType;
		}

		protected override object Inject(ref InjectionContext context)
		{
			var value = context.Container.Resolver.Resolve(context);
			wrapper.Set(ref context.Instance, value);

			return value;
		}

		public override string ToString()
		{
			return string.Format("{0}({1}.{2})", GetType().Name, provider.DeclaringType.Name, provider.Name);
		}
	}
}
