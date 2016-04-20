using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;
using UnityEngine.Assertions;
using System.Runtime.Serialization;

namespace Pseudo.Injection.Internal
{
	public class Instantiator : IInstantiator
	{
		static readonly IConstructorSelector defaultSelector = new ConstructorSelector();

		public IContainer Container
		{
			get { return container; }
		}
		public IConstructorSelector ConstructorSelector
		{
			get { return selector; }
			set { selector = value ?? defaultSelector; }
		}

		readonly IContainer container;
		IConstructorSelector selector = defaultSelector;

		public Instantiator(IContainer container)
		{
			this.container = container;
		}

		public object Instantiate(InjectionContext context)
		{
			SetupContext(ref context);

			var constructor = GetConstructor(ref context);

			if (constructor != null)
			{
				context.Instance = constructor.Inject(context);
				context.Container.Injector.Inject(context);

				return context.Instance;
			}
			// Try instantiating with parent.
			else if (context.Container.Parent != null)
				return context.Container.Parent.Instantiator.Instantiate(context);
			else
				throw new ArgumentException(string.Format("No valid constructor was found for context {0}.", context));
		}

		public bool CanInstantiate(InjectionContext context)
		{
			SetupContext(ref context);

			return
				GetConstructor(ref context) != null ||
				(context.Container.Parent != null && context.Container.Parent.Instantiator.CanInstantiate(context));
		}

		void SetupContext(ref InjectionContext context)
		{
			context.Container = container;
		}

		IInjectableConstructor GetConstructor(ref InjectionContext context)
		{
			if (context.DeclaringType == null || !context.DeclaringType.IsConcrete())
				return null;

			return selector.Select(context, context.Container.Analyzer.Analyze(context.DeclaringType).Constructors);
		}
	}
}
