using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;

namespace Pseudo.Internal.Injection
{
	public class InjectableConstructorParameter : IInjectableParameter
	{
		public MemberInfo Member
		{
			get { return constructor; }
		}

		public ParameterInfo Parameter
		{
			get { return parameter; }
		}

		readonly ConstructorInfo constructor;
		readonly ParameterInfo parameter;
		readonly InjectAttribute attribute;

		public InjectableConstructorParameter(ConstructorInfo constructor, ParameterInfo parameter)
		{
			this.constructor = constructor;
			this.parameter = parameter;

			attribute = (InjectAttribute)parameter.GetCustomAttributes(typeof(InjectAttribute), true).First() ?? new InjectAttribute();
		}

		public void Inject(object instance, object[] arguments, int index, IResolver resolver)
		{
			if (!attribute.Optional || CanInject(resolver))
				arguments[index] = resolver.Resolve(new InjectionContext
				{
					Resolver = resolver,
					Additional = InjectionUtility.Empty,
					Type = InjectionContext.Types.Constructor,
					Instance = instance,
					ContractType = parameter.ParameterType,
					DeclaringType = constructor.DeclaringType,
					Optional = attribute.Optional,
					Identifier = attribute.Identifier
				});
		}

		public bool CanInject(IResolver resolver)
		{
			return resolver.CanResolve(parameter.ParameterType);
		}
	}
}
