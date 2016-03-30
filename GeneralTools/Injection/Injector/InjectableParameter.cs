using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;

namespace Pseudo.Internal.Injection
{
	public class InjectableParameter : IInjectableParameter
	{
		public ParameterInfo Parameter
		{
			get { return parameter; }
		}

		readonly ParameterInfo parameter;
		readonly InjectAttribute attribute;

		public InjectableParameter(ParameterInfo parameter)
		{
			this.parameter = parameter;

			attribute = (InjectAttribute)parameter.GetCustomAttributes(typeof(InjectAttribute), true).First() ?? new InjectAttribute();
		}

		public object Resolve(InjectionContext context)
		{
			SetupContext(ref context);

			if (!attribute.Optional || context.Binder.Resolver.CanResolve(context))
				return context.Binder.Resolver.Resolve(context);

			return TypeUtility.GetDefaultValue(parameter.ParameterType);
		}

		public bool CanResolve(ref InjectionContext context)
		{
			return context.Binder.Resolver.CanResolve(parameter.ParameterType);
		}

		void SetupContext(ref InjectionContext context)
		{
			context.ContextType |= InjectionContext.ContextTypes.Parameter;
			context.ContractType = parameter.ParameterType;
			context.Parameter = parameter;
			context.Attribute = attribute;
		}
	}
}
