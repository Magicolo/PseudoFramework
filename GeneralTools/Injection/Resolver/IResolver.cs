using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Internal.Injection;

namespace Pseudo
{
	public interface IResolver
	{
		IBinder Binder { get; }

		object Resolve(Type contractType);
		object Resolve(InjectionContext context);
		TContract Resolve<TContract>() where TContract : class;
		IEnumerable<object> ResolveAll(Type contractType);
		IEnumerable<TContract> ResolveAll<TContract>() where TContract : class;
		bool CanResolve(Type contractType);
		bool CanResolve(params Type[] contractTypes);
		bool CanResolve(InjectionContext context);
	}
}
