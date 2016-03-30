using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using UnityEngine.Assertions;

namespace Pseudo.Internal.Injection
{
	public class Injector : IInjector
	{
		public IBinder Binder
		{
			get { return binder; }
		}

		readonly IBinder binder;

		public Injector(IBinder binder)
		{
			this.binder = binder;
		}

		public void Inject(object instance)
		{
			Inject(new InjectionContext
			{
				Binder = binder,
				Instance = instance
			});
		}

		public void Inject(params object[] instances)
		{
			for (int i = 0; i < instances.Length; i++)
				Inject(instances[i]);
		}

		public void Inject(InjectionContext context)
		{
			Assert.IsNotNull(context.Binder);
			Assert.IsNotNull(context.Instance);

			bool isInjectable = context.Instance is IInjectable;

			if (isInjectable)
				((IInjectable)context.Instance).OnPreInject(binder);

			var injectables = InjectionUtility.GetInjectableMembers(context.Instance.GetType());

			for (int i = 0; i < injectables.Length; i++)
				injectables[i].Inject(context);

			if (isInjectable)
				((IInjectable)context.Instance).OnPostInject(binder);
		}
	}
}
