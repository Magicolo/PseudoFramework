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

		public void Inject(object instance, IResolver resolver)
		{
			if (!attribute.Optional || CanInject(resolver))
				field.SetValue(instance, resolver.Resolve(new InjectionContext
				{
					Resolver = resolver,
					Additional = InjectionUtility.Empty,
					Type = InjectionContext.Types.Field,
					Instance = instance,
					ContractType = field.FieldType,
					DeclaringType = field.DeclaringType,
					Optional = attribute.Optional,
					Identifier = attribute.Identifier
				}));
		}

		public bool CanInject(IResolver resolver)
		{
			return resolver.CanResolve(field.FieldType);
		}
	}
}
