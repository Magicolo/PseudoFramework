using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Injection
{
	public static class ContainerExtensions
	{
		public static object Get(this IContainer container, Type type)
		{
			if (container.Resolver.CanResolve(type))
				return container.Resolver.Resolve(type);
			else if (container.Parent != null && container.Parent.Resolver.CanResolve(type))
				return container.Parent.Resolver.Resolve(type);
			else if (container.Instantiator.CanInstantiate(type))
				return container.Instantiator.Instantiate(type);
			else if (container.Parent != null && container.Parent.Instantiator.CanInstantiate(type))
				return container.Parent.Instantiator.Instantiate(type);
			else
				return null;
		}

		public static T Get<T>(this IContainer container)
		{
			return (T)container.Get(typeof(T));
		}

		public static bool CanGet(this IContainer container, Type type)
		{
			return
				container.Resolver.CanResolve(type) ||
				(container.Parent != null && container.Parent.Resolver.CanResolve(type)) ||
				container.Instantiator.CanInstantiate(type) ||
				(container.Parent != null && container.Parent.Instantiator.CanInstantiate(type));
		}

		public static bool CanGet<T>(this IContainer container)
		{
			return container.CanGet(typeof(T));
		}
	}
}
