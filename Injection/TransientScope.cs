using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Injection.Internal
{
	public class TransientScope : IInjectionScope
	{
		public object GetInstance(IInjectionFactory factory, InjectionContext context)
		{
			return factory.Create(context);
		}
	}

	public class TransientScope<TConcrete> : TransientScope, IInjectionScope<TConcrete>
	{
		public TConcrete GetInstance(IInjectionFactory<TConcrete> factory, InjectionContext context)
		{
			return (TConcrete)GetInstance((IInjectionFactory)factory, context);
		}
	}
}
