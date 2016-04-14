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
		static readonly ConstructorSelector defaultSelector = new ConstructorSelector();

		public IContainer Container
		{
			get { return container; }
		}
		public IConstructorSelector Selector
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
			Assert.IsNotNull(context.DeclaringType);
			Assert.IsTrue(context.DeclaringType.IsConcrete());

			var constructor = GetValidConstructor(ref context);

			if (constructor == null)
				throw new ArgumentException(string.Format("No valid constructor was found for type {0}.", context.DeclaringType.Name));

			context.Instance = constructor.Inject(context);
			container.Injector.Inject(context);

			return context.Instance;
		}

		public object Instantiate(Type concreteType)
		{
			return Instantiate(new InjectionContext
			{
				ContextType = InjectionContext.ContextTypes.Constructor,
				DeclaringType = concreteType,
				Container = container
			});
		}

		public TConcrete Instantiate<TConcrete>()
		{
			return (TConcrete)Instantiate(typeof(TConcrete));
		}

		public bool CanInstantiate(InjectionContext context)
		{
			if (context.DeclaringType == null || !context.DeclaringType.IsConcrete())
				return false;

			return GetValidConstructor(ref context) != null;
		}

		public bool CanInstantiate(Type concreteType)
		{
			return CanInstantiate(new InjectionContext
			{
				ContextType = InjectionContext.ContextTypes.Constructor,
				DeclaringType = concreteType,
				Container = container
			});
		}

		public bool CanInstantiate<TConcrete>()
		{
			return CanInstantiate(typeof(TConcrete));
		}

		IInjectableConstructor GetValidConstructor(ref InjectionContext context)
		{
			return selector.Select(context, InjectionUtility.GetInjectionInfo(context.DeclaringType).Constructors);
		}
	}
}
