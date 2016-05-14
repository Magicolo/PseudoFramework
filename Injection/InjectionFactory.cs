using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Injection.Internal
{
	public class InjectionMethodFactory<TConcrete> : InjectionFactoryBase<TConcrete>
	{
		readonly InjectionMethod<TConcrete> method;

		public InjectionMethodFactory(InjectionMethod<TConcrete> method)
		{
			this.method = method;
		}

		public override TConcrete Create(InjectionContext argument)
		{
			var instance = method(argument);
			argument.Instance = instance;
			argument.Container.Injector.Inject(argument);

			return instance;
		}
	}
}
