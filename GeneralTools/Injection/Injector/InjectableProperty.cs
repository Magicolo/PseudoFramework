using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;

namespace Pseudo.Internal.Injection
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

		public void Inject(object instance, IResolver resolver)
		{
			if (!attribute.Optional || CanInject(resolver))
				property.SetValue(instance, resolver.Resolve(new InjectionContext
				{
					Resolver = resolver,
					Additional = InjectionUtility.Empty,
					Type = InjectionContext.Types.Property,
					Instance = instance,
					ContractType = property.PropertyType,
					DeclaringType = property.DeclaringType,
					Optional = attribute.Optional,
					Identifier = attribute.Identifier
				}), null);
		}

		public bool CanInject(IResolver resolver)
		{
			return resolver.CanResolve(property.PropertyType);
		}
	}
}
