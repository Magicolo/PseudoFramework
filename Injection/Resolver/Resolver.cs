using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using UnityEngine.Assertions;

namespace Pseudo.Injection.Internal
{
	public class Resolver : IResolver
	{
		public IContainer Container
		{
			get { return container; }
		}

		readonly IContainer container;

		public Resolver(IContainer container)
		{
			this.container = container;
		}

		public object Resolve(InjectionContext context)
		{
			Assert.IsNotNull(context.Container);

			var binding = context.Container.Binder.GetBinding(context);

			if (binding != null)
				return binding.Scope.GetInstance(binding.Factory, context);
			else if (context.Container.Parent != null)
				return context.Container.Parent.Resolver.Resolve(context);

			throw new ArgumentException(string.Format("No binding was found for context {0}.", context));
		}

		public IEnumerable<object> ResolveAll(InjectionContext context)
		{
			Assert.IsNotNull(context.Container);

			if (context.Container.Parent == null)
				return context.Container.Binder.GetBindings(context)
					.Select(b => b.Scope.GetInstance(b.Factory, context));
			else
				return context.Container.Binder.GetBindings(context)
					.Select(b => b.Scope.GetInstance(b.Factory, context))
					.Concat(context.Container.Parent.Resolver.ResolveAll(context));
		}

		public bool CanResolve(InjectionContext context)
		{
			if (context.Container == null)
				return false;

			return
				context.Container.Binder.GetBinding(context) != null ||
				(context.Container.Parent != null && context.Container.Parent.Resolver.CanResolve(context));
		}
	}
}
