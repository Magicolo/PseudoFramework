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
			Assert.IsNotNull(context.Container);
			Assert.IsNotNull(context.DeclaringType);
			Assert.IsTrue(context.DeclaringType.IsConcrete());

			var constructor = selector.Select(context, context.Container.Analyzer.Analyze(context.DeclaringType).Constructors);

			if (constructor == null)
				throw new ArgumentException(string.Format("No valid constructor was found for type {0}.", context.DeclaringType.Name));

			context.Instance = constructor.Inject(context);
			context.Container.Injector.Inject(context);

			return context.Instance;
		}

		public bool CanInstantiate(InjectionContext context)
		{
			if (context.Container == null || context.DeclaringType == null || !context.DeclaringType.IsConcrete())
				return false;

			return selector.Select(context, context.Container.Analyzer.Analyze(context.DeclaringType).Constructors) != null;
		}
	}
}
