using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using UnityEngine.Assertions;

namespace Pseudo.Injection
{
	public static class InjectorExtensions
	{
		public static void Inject(this IInjector injector, object instance)
		{
			Assert.IsNotNull(instance);

			injector.Inject(new InjectionContext
			{
				Container = injector.Container,
				Instance = instance,
				DeclaringType = instance.GetType()
			});
		}
	}
}
