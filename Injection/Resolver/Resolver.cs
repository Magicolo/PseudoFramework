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
			SetupContext(ref context);

			var binding = GetBinding(ref context);

			if (binding != null)
				return binding.Scope.GetInstance(binding.Factory, context);
			// Try resolving with parent.
			else if (context.Container.Parent != null)
				return context.Container.Parent.Resolver.Resolve(context);
			else
				throw new ArgumentException(string.Format("No binding was found for context {0}.", context));
		}

		public IEnumerable<object> ResolveAll(InjectionContext context)
		{
			SetupContext(ref context);

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
			SetupContext(ref context);

			return
				GetBinding(ref context) != null ||
				(context.Container.Parent != null && context.Container.Parent.Resolver.CanResolve(context));
		}

		void SetupContext(ref InjectionContext context)
		{
			context.Container = container;
		}

		IBinding GetBinding(ref InjectionContext context)
		{
			if (context.ContractType == null)
				return null;

			return context.Container.Binder.GetBinding(context);
		}
	}
}
