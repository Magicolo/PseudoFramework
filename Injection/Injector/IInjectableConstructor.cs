using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;

namespace Pseudo.Injection.Internal
{
	public interface IInjectableConstructor
	{
		ConstructorInfo Constructor { get; }
		IInjectableParameter[] Parameters { get; }

		object Inject(InjectionContext context);
		bool CanInject(InjectionContext context);
	}
}
