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
		IResolver Parent { get; }

		object Resolve(Type contractType);
		object Resolve(Type contractType, params object[] additional);
		object Resolve(InjectionContext context);
		TContract Resolve<TContract>() where TContract : class;
		TContract Resolve<TContract>(params object[] additional) where TContract : class;
		bool CanResolve(Type contractType);
		bool CanResolve(params Type[] contractTypes);
		bool CanResolve(InjectionContext context);
	}
}
