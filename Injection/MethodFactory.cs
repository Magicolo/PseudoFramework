using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Injection.Internal
{
	public class MethodFactory<TConcrete> : IInjectionFactory<TConcrete>
	{
		readonly InjectionMethod<TConcrete> method;

		public MethodFactory(InjectionMethod<TConcrete> method)
		{
			this.method = method;
		}

		public object Create(InjectionContext context)
		{
			context.Instance = method(context);
			context.Container.Injector.Inject(context);

			return context.Instance;
		}

		TConcrete IInjectionFactory<TConcrete>.Create(InjectionContext context)
		{
			return (TConcrete)Create(context);
		}
	}
}
