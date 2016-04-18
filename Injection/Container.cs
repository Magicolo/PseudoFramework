using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Injection.Internal
{
	public class Container : IContainer
	{
		static readonly ITypeAnalyzer defaultAnalyzer = new TypeAnalyzer();

		public IContainer Parent
		{
			get { return parent; }
		}
		public IBinder Binder
		{
			get { return binder; }
		}
		public IResolver Resolver
		{
			get { return resolver; }
		}
		public IInjector Injector
		{
			get { return injector; }
		}
		public IInstantiator Instantiator
		{
			get { return instantiator; }
		}
		public ITypeAnalyzer Analyzer
		{
			get { return analyzer; }
			set { analyzer = value ?? defaultAnalyzer; }
		}

		readonly IContainer parent;
		readonly IBinder binder;
		readonly IResolver resolver;
		readonly IInjector injector;
		readonly IInstantiator instantiator;

		ITypeAnalyzer analyzer = defaultAnalyzer;

		public Container() : this(null, null, null, null, null) { }

		public Container(IContainer parent) : this(parent, null, null, null, null) { }

		public Container(IContainer parent, IBinder binder, IResolver resolver, IInjector injector, IInstantiator instantiator)
		{
			this.parent = parent;
			this.binder = binder ?? new Binder(this);
			this.resolver = resolver ?? new Resolver(this);
			this.injector = injector ?? new Injector(this);
			this.instantiator = instantiator ?? new Instantiator(this);

			Binder.Bind(GetType(), typeof(IContainer)).ToInstance(this);
			Binder.Bind(this.binder.GetType(), typeof(IBinder)).ToInstance(this.binder);
			Binder.Bind(this.resolver.GetType(), typeof(IResolver)).ToInstance(this.resolver);
			Binder.Bind(this.injector.GetType(), typeof(IInjector)).ToInstance(this.injector);
			Binder.Bind(this.instantiator.GetType(), typeof(IInstantiator)).ToInstance(this.instantiator);
		}

		public object Get(InjectionContext context)
		{
			if (resolver.CanResolve(context))
				return resolver.Resolve(context);
			else if (parent != null && parent.Resolver.CanResolve(context))
				return parent.Resolver.Resolve(context);
			else if (instantiator.CanInstantiate(context))
				return instantiator.Instantiate(context);
			else if (parent != null && parent.Instantiator.CanInstantiate(context))
				return parent.Instantiator.Instantiate(context);
			else
				return null;
		}
	}
}
