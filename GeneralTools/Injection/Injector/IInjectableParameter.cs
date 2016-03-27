using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;

namespace Pseudo.Internal.Injection
{
	public interface IInjectableParameter
	{
		ParameterInfo Parameter { get; }

		object Resolve(InjectionContext context);
		bool CanResolve(ref InjectionContext context);
	}
}
