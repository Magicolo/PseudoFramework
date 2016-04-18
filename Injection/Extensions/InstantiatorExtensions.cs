using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Injection.Internal;

namespace Pseudo.Injection
{
	public static class InstantiatorExtensions
	{
		public static object Instantiate(this IInstantiator instantiator, Type concreteType, params object[] arguments)
		{
			return instantiator.Instantiate(new InjectionContext
			{
				Container = instantiator.Container,
				DeclaringType = concreteType,
				Arguments = arguments
			});
		}

		public static object Instantiate(this IInstantiator instantiator, Type concreteType)
		{
			return instantiator.Instantiate(concreteType, InjectionUtility.EmptyArguments);
		}

		public static TConcrete Instantiate<TConcrete>(this IInstantiator instantiator, params object[] arguments)
		{
			return (TConcrete)instantiator.Instantiate(typeof(TConcrete), arguments);
		}

		public static TConcrete Instantiate<TConcrete>(this IInstantiator instantiator)
		{
			return (TConcrete)instantiator.Instantiate(typeof(TConcrete));
		}

		public static bool CanInstantiate(this IInstantiator instantiator, Type concreteType, params object[] arguments)
		{
			return instantiator.CanInstantiate(new InjectionContext
			{
				Container = instantiator.Container,
				DeclaringType = concreteType,
				Arguments = arguments
			});
		}

		public static bool CanInstantiate(this IInstantiator instantiator, Type concreteType)
		{
			return instantiator.CanInstantiate(concreteType, InjectionUtility.EmptyArguments);
		}

		public static bool CanInstantiate<TConcrete>(this IInstantiator instantiator, params object[] arguments)
		{
			return instantiator.CanInstantiate(typeof(TConcrete), arguments);
		}

		public static bool CanInstantiate<TConcrete>(this IInstantiator instantiator)
		{
			return instantiator.CanInstantiate(typeof(TConcrete));
		}
	}
}
