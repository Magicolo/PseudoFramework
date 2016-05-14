using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Injection
{
	public abstract class InjectionFactoryBase<TConcrete> : FactoryBase<InjectionContext, TConcrete>, IInjectionFactory<TConcrete>
	{
		object IFactory<InjectionContext, object>.Create(InjectionContext argument)
		{
			return Create(argument);
		}
	}
}
