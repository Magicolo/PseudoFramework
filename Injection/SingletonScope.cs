using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Injection.Internal
{
	public class SingletonScope : IInjectionScope
	{
		object instance;
		bool created;

		public object GetInstance(IInjectionFactory factory, InjectionContext context)
		{
			if (!created)
			{
				instance = factory.Create(context);
				created = true;
			}

			return instance;
		}
	}

	public class SingletonScope<TConcrete> : SingletonScope, IInjectionScope<TConcrete>
	{
		public TConcrete GetInstance(IInjectionFactory<TConcrete> factory, InjectionContext context)
		{
			return (TConcrete)GetInstance((IInjectionFactory)factory, context);
		}
	}
}
