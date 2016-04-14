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

		object Resolve(Type contractType);
		object Resolve(InjectionContext context);
		TContract Resolve<TContract>();
		IEnumerable<object> ResolveAll(Type contractType);
		IEnumerable<object> ResolveAll(InjectionContext context);
		IEnumerable<TContract> ResolveAll<TContract>();
		bool CanResolve(Type contractType);
		bool CanResolve<TContract>();
		bool CanResolve(InjectionContext context);
	}
}
