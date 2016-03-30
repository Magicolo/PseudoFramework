using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;
using UnityEngine.Assertions;
using System.Runtime.Serialization;

namespace Pseudo.Internal.Injection
{
	public class Instantiator : IInstantiator
	{
		public IBinder Binder
		{
			get { return binder; }
		}

		readonly IBinder binder;

		public Instantiator(IBinder binder)
		{
			this.binder = binder;
		}

		public object Instantiate(Type concreteType)
		{
			Assert.IsTrue(!concreteType.IsInterface && !concreteType.IsAbstract);

			var context = new InjectionContext { Binder = binder };
			var constructor = GetValidConstructor(ref context, concreteType);

			if (constructor == null)
				throw new ArgumentException(string.Format("No valid constructor was found for type {0}.", concreteType.Name));

			context.Instance = constructor.Inject(context);
			binder.Injector.Inject(context);

			return context.Instance;
		}

		public T Instantiate<T>()
		{
			return (T)Instantiate(typeof(T));
		}

		IInjectableConstructor GetValidConstructor(ref InjectionContext context, Type concreteType)
		{
			var constructors = InjectionUtility.GetInjectableConstructors(concreteType);

			for (int i = 0; i < constructors.Length; i++)
			{
				var constructor = constructors[i];

				if (constructor.CanInject(context))
					return constructor;
			}

			return constructors.First();
		}
	}
}
