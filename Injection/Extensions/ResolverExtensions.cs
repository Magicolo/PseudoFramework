using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Injection
{
	public static class ResolverExtensions
	{
		public static object Resolve(this IResolver resolver, Type contractType, object identifier)
		{
			return resolver.Resolve(new InjectionContext
			{
				Container = resolver.Container,
				ContractType = contractType,
				Identifier = identifier
			});
		}

		public static object Resolve(this IResolver resolver, Type contractType)
		{
			return resolver.Resolve(contractType, "");
		}

		public static TContract Resolve<TContract>(this IResolver resolver)
		{
			return (TContract)resolver.Resolve(typeof(TContract));
		}

		public static TContract Resolve<TContract>(this IResolver resolver, object identifier)
		{
			return (TContract)resolver.Resolve(typeof(TContract), identifier);
		}

		public static IEnumerable<object> ResolveAll(this IResolver resolver, Type contractType, object identifier)
		{
			return resolver.ResolveAll(new InjectionContext
			{
				Container = resolver.Container,
				ContractType = contractType,
				Identifier = identifier
			});
		}

		public static IEnumerable<object> ResolveAll(this IResolver resolver, Type contractType)
		{
			return resolver.ResolveAll(contractType, "");
		}

		public static IEnumerable<TContract> ResolveAll<TContract>(this IResolver resolver)
		{
			return resolver.ResolveAll(typeof(TContract)).Cast<TContract>();
		}

		public static IEnumerable<TContract> ResolveAll<TContract>(this IResolver resolver, object identifier)
		{
			return resolver.ResolveAll(typeof(TContract), identifier).Cast<TContract>();
		}

		public static bool CanResolve(this IResolver resolver, Type contractType, object identifier)
		{
			return resolver.CanResolve(new InjectionContext
			{
				Container = resolver.Container,
				ContractType = contractType,
				Identifier = identifier
			});
		}

		public static bool CanResolve(this IResolver resolver, Type contractType)
		{
			return resolver.CanResolve(contractType, "");
		}

		public static bool CanResolve<TContract>(this IResolver resolver)
		{
			return resolver.CanResolve(typeof(TContract));
		}

		public static bool CanResolve<TContract>(this IResolver resolver, object identifier)
		{
			return resolver.CanResolve(typeof(TContract), identifier);
		}
	}
}
