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
	public class InjectableField : InjectableMemberBase<FieldInfo>, IInjectableField
	{
		public InjectableField(FieldInfo field) : base(field) { }

		protected override void SetupContext(ref InjectionContext context)
		{
			base.SetupContext(ref context);

			context.Type = ContextTypes.Field;
			context.ContractType = member.FieldType;
		}

		protected override bool CanInject(ref InjectionContext context)
		{
			return context.Container.Resolver.CanResolve(context);
		}

		protected override object Inject(ref InjectionContext context)
		{
			var value = context.Container.Resolver.Resolve(context);
			member.SetValue(context.Instance, value);

			return value;
		}

		public override string ToString()
		{
			return string.Format("{0}({1}.{2})", GetType().Name, member.DeclaringType.Name, member.Name);
		}
	}
}
