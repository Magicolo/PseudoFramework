using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Injection.Internal;

namespace Pseudo.Injection
{
	public interface IResolver
	{
		IContainer Container { get; }

		object Resolve(InjectionContext context);
		IEnumerable<object> ResolveAll(InjectionContext context);
		bool CanResolve(InjectionContext context);
	}
}
