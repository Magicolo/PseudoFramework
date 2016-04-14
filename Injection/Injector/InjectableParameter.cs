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
	public class InjectableParameter : IInjectableParameter
	{
		public ParameterInfo Parameter
		{
			get { return parameter; }
		}
		public InjectAttribute Attribute
		{
			get { return attribute; }
		}

		readonly ParameterInfo parameter;
		readonly InjectAttribute attribute;

		public InjectableParameter(ParameterInfo parameter)
		{
			this.parameter = parameter;

			attribute = parameter.GetAttribute<InjectAttribute>(true) ?? new InjectAttribute();
		}

		public object Inject(InjectionContext context)
		{
			SetupContext(ref context);

			if (!attribute.Optional || CanInject(context))
				return context.Container.Resolver.Resolve(context);

			return null;
		}

		public bool CanInject(InjectionContext context)
		{
			SetupContext(ref context);

			return context.Container.Resolver.CanResolve(context);
		}

		void SetupContext(ref InjectionContext context)
		{
			context.ContextType |= InjectionContext.ContextTypes.Parameter;
			context.ContractType = parameter.ParameterType;
			context.Identifier = attribute.Identifier;
			context.Optional = attribute.Optional;
		}
	}
}
