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
		static readonly IInjectionInterceptor defaultInterceptor = new InjectionInterceptor();

		public IContainer Container
		{
			get { return container; }
		}
		public List<IInjectionInterceptor> Interceptors
		{
			get { return interceptors; }
		}

		readonly IContainer container;
		readonly List<IInjectionInterceptor> interceptors = new List<IInjectionInterceptor>();

		public Injector(IContainer container)
		{
			this.container = container;
		}

		public void Inject(InjectionContext context)
		{
			Assert.IsNotNull(context.Container);
			Assert.IsNotNull(context.Instance);

			var info = context.Container.Analyzer.Analyze(context.Instance.GetType());
			var interceptor = GetInterceptor(ref context, info);
			interceptor.Inject(context, info);
		}

		IInjectionInterceptor GetInterceptor(ref InjectionContext context, ITypeInfo info)
		{
			for (int i = 0; i < interceptors.Count; i++)
			{
				var interceptor = interceptors[i];

				if (interceptor.CanInject(context, info))
					return interceptor;
			}

			return defaultInterceptor;
		}
	}
}
