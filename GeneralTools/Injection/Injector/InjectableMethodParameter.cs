using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;

namespace Pseudo.Internal.Injection
{
	public class InjectableMethodParameter : IInjectableParameter
	{
		public MemberInfo Member
		{
			get { return method; }
		}

		public ParameterInfo Parameter
		{
			get { return parameter; }
		}

		readonly MethodInfo method;
		readonly ParameterInfo parameter;
		readonly InjectAttribute attribute;

		public InjectableMethodParameter(MethodInfo method, ParameterInfo parameter)
		{
			this.method = method;
			this.parameter = parameter;

			attribute = (InjectAttribute)parameter.GetCustomAttributes(typeof(InjectAttribute), true).First() ?? new InjectAttribute();
		}

		public void Inject(object instance, object[] arguments, int index, IResolver resolver)
		{
			if (!attribute.Optional || resolver.CanResolve(parameter.ParameterType))
				arguments[index] = resolver.Resolve(new InjectionContext
				{
					Resolver = resolver,
					Additional = InjectionUtility.Empty,
					Type = InjectionContext.Types.Method,
					Instance = instance,
					ContractType = parameter.ParameterType,
					DeclaringType = method.DeclaringType,
					Optional = attribute.Optional,
					Identifier = attribute.Identifier
				});
		}

		public bool CanInject(IResolver resolver)
		{
			throw new NotImplementedException();
		}
	}
}
