using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;

namespace Pseudo.Internal.Injection
{
	public class Instantiator : IInstantiator
	{
		readonly IInjector injector;
		readonly IResolver resolver;

		public Instantiator(IInjector injector, IResolver resolver)
		{
			this.injector = injector;
			this.resolver = resolver;
		}

		public object Instantiate(Type concreteType, params object[] additional)
		{
			var constructor = GetValidConstructor(concreteType, additional);

			if (constructor == null)
				throw new ArgumentException(string.Format("No valid constructor was found for type {0} with additional arguments {1}.", concreteType.Name, PDebug.ToString(additional)));

			var instance = constructor.Inject(resolver, additional);
			injector.Inject(instance);

			return instance;
		}

		public T Instantiate<T>(params object[] arguments) where T : class
		{
			return (T)Instantiate(typeof(T), arguments);
		}

		IInjectableConstructor GetValidConstructor(Type concreteType, object[] additional)
		{
			var constructors = InjectionUtility.GetInjectableConstructors(concreteType);

			for (int i = 0; i < constructors.Length; i++)
			{
				var constructor = constructors[i];

				if (constructor.CanInject(resolver, additional))
					return constructor;
			}

			return constructors.First();
		}
	}
}
