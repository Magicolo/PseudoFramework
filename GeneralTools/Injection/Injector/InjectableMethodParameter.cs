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

		public void Inject(InjectionContext context, object[] arguments, int index)
		{
			SetupContext(ref context);

			if (!attribute.Optional || context.Binder.Resolver.CanResolve(context))
				arguments[index] = context.Binder.Resolver.Resolve(context);
		}

		public bool CanInject(ref InjectionContext context)
		{
			return context.Binder.Resolver.CanResolve(parameter.ParameterType);
		}

		void SetupContext(ref InjectionContext context)
		{
			context.Type = InjectionContext.Types.MethodParameter;
			context.ContractType = parameter.ParameterType;
			context.Parameter = parameter;
			context.Optional = attribute.Optional;
			context.Identifier = attribute.Identifier;
		}
	}
}
