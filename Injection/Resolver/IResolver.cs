using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

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
