using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

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
			var binding = container.Binder.GetBinding(context);

			if (binding != null)
				return binding.Factory.Create(context);
			else if (container.Parent != null)
				return container.Parent.Resolver.Resolve(context);

			throw new ArgumentException(string.Format("No binding was found for context {0}.", context));
		}

		public object Resolve(Type contractType)
		{
			var binding = container.Binder.GetBinding(contractType);

			if (binding != null)
				return binding.Factory.Create(new InjectionContext
				{
					Container = container,
					ContractType = contractType
				});
			else if (container.Parent != null)
				return container.Parent.Resolver.Resolve(contractType);

			throw new ArgumentException(string.Format("No binding was found for type {0}.", contractType.Name));
		}

		public TContract Resolve<TContract>()
		{
			return (TContract)Resolve(typeof(TContract));
		}

		public IEnumerable<object> ResolveAll(InjectionContext context)
		{
			if (container.Parent == null)
				return container.Binder.GetBindings(context)
					.Select(b => b.Factory.Create(context));
			else
				return container.Binder.GetBindings(context)
					.Select(b => b.Factory.Create(context))
					.Concat(container.Parent.Resolver.ResolveAll(context));
		}

		public IEnumerable<object> ResolveAll(Type contractType)
		{
			var context = new InjectionContext
			{
				Container = container,
				ContractType = contractType
			};

			if (container.Parent == null)
				return container.Binder.GetBindings(contractType)
					.Select(b => b.Factory.Create(context));
			else
				return container.Binder.GetBindings(contractType)
					.Select(b => b.Factory.Create(context))
					.Concat(container.Parent.Resolver.ResolveAll(contractType));
		}

		public IEnumerable<TContract> ResolveAll<TContract>()
		{
			var context = new InjectionContext
			{
				Container = container,
				ContractType = typeof(TContract)
			};

			if (container.Parent == null)
				return container.Binder.GetBindings(context)
					.Select(data => (TContract)data.Factory.Create(context));
			else
				return container.Binder.GetBindings(context)
					.Select(data => (TContract)data.Factory.Create(context))
					.Concat(container.Parent.Resolver.ResolveAll<TContract>());
		}

		public bool CanResolve(InjectionContext context)
		{
			return
				container.Binder.GetBinding(context) != null ||
				(container.Parent != null && container.Parent.Resolver.CanResolve(context));
		}

		public bool CanResolve(Type contractType)
		{
			return
				container.Binder.HasBinding(contractType) ||
				(container.Parent != null && container.Parent.Resolver.CanResolve(contractType));
		}

		public bool CanResolve<TContract>()
		{
			return CanResolve(typeof(TContract));
		}
	}
}
