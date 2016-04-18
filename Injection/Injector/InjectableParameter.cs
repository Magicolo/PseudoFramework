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
	public class InjectableParameter : InjectableElementBase, IInjectableParameter
	{
		public ParameterInfo Parameter
		{
			get { return parameter; }
		}

		readonly ParameterInfo parameter;

		public InjectableParameter(ParameterInfo parameter) : base(parameter)
		{
			this.parameter = parameter;
		}

		protected override void SetupContext(ref InjectionContext context)
		{
			base.SetupContext(ref context);

			context.Type |= ContextTypes.Parameter;
			context.ContractType = parameter.ParameterType;
		}

		protected override bool CanInject(ref InjectionContext context)
		{
			return parameter.IsOptional || context.Container.Resolver.CanResolve(context);
		}

		protected override object Inject(ref InjectionContext context)
		{
			if (context.Container.Resolver.CanResolve(context))
				return context.Container.Resolver.Resolve(context);
			else if (parameter.IsOptional)
				return parameter.DefaultValue;
			else if (!attribute.Optional)
				return context.Container.Resolver.Resolve(context);

			return null;
		}
	}
}
