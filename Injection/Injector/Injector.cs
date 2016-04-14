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
		public IContainer Container
		{
			get { return container; }
		}

		readonly IContainer container;

		public Injector(IContainer container)
		{
			this.container = container;
		}

		public void Inject(InjectionContext context)
		{
			Assert.IsNotNull(context.Container);
			Assert.IsNotNull(context.Instance);

			var info = InjectionUtility.GetInjectionInfo(context.Instance.GetType());
			var interceptor = context.Instance as IInjectionInterceptor;

			if (interceptor == null)
			{
				// Inject Fields
				for (int i = 0; i < info.Fields.Length; i++)
					info.Fields[i].Inject(context);

				// Inject Properties
				for (int i = 0; i < info.Properties.Length; i++)
					info.Properties[i].Inject(context);

				// Inject Methods
				for (int i = 0; i < info.Methods.Length; i++)
					info.Methods[i].Inject(context);
			}
			else
				interceptor.OnInject(context, info);
		}

		public void Inject(object instance)
		{
			Assert.IsNotNull(instance);

			Inject(new InjectionContext
			{
				Container = container,
				Instance = instance,
				DeclaringType = instance.GetType()
			});
		}
	}
}
