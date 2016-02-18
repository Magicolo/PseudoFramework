using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;

namespace Pseudo.Internal.Injection
{
	public class InjectableField : IInjectableMember
	{
		public MemberInfo Member
		{
			get { return field; }
		}

		readonly FieldInfo field;
		readonly InjectAttribute attribute;

		public InjectableField(FieldInfo field)
		{
			this.field = field;

			attribute = (InjectAttribute)field.GetCustomAttributes(typeof(InjectAttribute), true).First() ?? new InjectAttribute();
		}

		public void Inject(InjectionContext context)
		{
			SetupContext(ref context);

			if (!context.Optional || context.Binder.Resolver.CanResolve(context))
				field.SetValue(context.Instance, context.Binder.Resolver.Resolve(context));
		}

		void SetupContext(ref InjectionContext context)
		{
			context.ContextType = InjectionContext.ContextTypes.Field;
			context.ContractType = field.FieldType;
			context.DeclaringType = field.DeclaringType;
			context.Member = field;
			context.Optional = attribute.Optional;
			context.Identifier = attribute.Identifier;
		}
	}
}
