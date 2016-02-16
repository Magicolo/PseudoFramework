using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal.Injection
{
	public class Injector : IInjector
	{
		readonly IResolver resolver;

		public Injector(IResolver resolver)
		{
			this.resolver = resolver;
		}

		public void Inject(object instance)
		{
			if (instance == null)
				throw new NullReferenceException("Instance was null.");

			var injectables = InjectionUtility.GetInjectableMembers(instance.GetType());

			for (int i = 0; i < injectables.Length; i++)
				injectables[i].Inject(instance, resolver);
		}

		public void Inject(GameObject gameObject, bool recursive = false)
		{
			var components = gameObject.GetComponents<Component>();

			for (int i = 0; i < components.Length; i++)
				Inject(components[i]);

			if (recursive)
			{
				var transform = gameObject.transform;

				for (int i = 0; i < transform.childCount; i++)
					Inject(transform.GetChild(i).gameObject, recursive);
			}
		}
	}
}
