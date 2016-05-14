using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Injection.Internal
{
	public class ConstructorSelector : IConstructorSelector
	{
		public IInjectableConstructor Select(InjectionContext context, IInjectableConstructor[] constructors)
		{
			for (int i = 0; i < constructors.Length; i++)
			{
				var constructor = constructors[i];

				if (constructor.CanInject(context))
					return constructor;
			}

			return constructors.First();
		}
	}
}
