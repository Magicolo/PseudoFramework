using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using UnityEngine.Assertions;

namespace Pseudo.Injection.Internal
{
	public class Injector : IInjector
	{
		static readonly IInjectorSelector defaultSelector = new InjectorSelector();

		public IContainer Container
		{
			get { return container; }
		}
		public IInjectorSelector InjectorSelector
		{
			get { return injectorSelector; }
			set { injectorSelector = value ?? defaultSelector; }
		}

		readonly IContainer container;
		IInjectorSelector injectorSelector = defaultSelector;

		public Injector(IContainer container)
		{
			this.container = container;
		}

		public void Inject(InjectionContext context)
		{
			Assert.IsNotNull(context.Instance);

			SetupContext(ref context);

			var info = context.Container.Analyzer.Analyze(context.Instance.GetType());
			var injector = InjectorSelector.Select(context, info);
			injector.Inject(context, info);
		}

		void SetupContext(ref InjectionContext context)
		{
			context.Container = container;
		}
	}
}
