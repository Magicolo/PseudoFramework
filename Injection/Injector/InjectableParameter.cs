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
	public class InjectableParameter : InjectableElementBase<ParameterInfo>, IInjectableParameter
	{
		public ParameterInfo Parameter
		{
			get { return provider; }
		}

		public InjectableParameter(ParameterInfo parameter) : base(parameter) { }

		protected override void SetupContext(ref InjectionContext context)
		{
			base.SetupContext(ref context);

			context.Type |= ContextTypes.Parameter;
			context.ContractType = provider.ParameterType;
		}

		protected override bool CanInject(ref InjectionContext context)
		{
			return provider.IsOptional || base.CanInject(ref context);
		}

		protected override object Inject(ref InjectionContext context)
		{
			if (context.Container.Resolver.CanResolve(context))
				return context.Container.Resolver.Resolve(context);
			else if (provider.IsOptional)
				return provider.DefaultValue;
			else if (!attribute.Optional)
				return context.Container.Resolver.Resolve(context);

			return null;
		}
	}
}
