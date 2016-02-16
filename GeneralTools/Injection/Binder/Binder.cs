﻿using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal.Injection
{
	public class Binder : IBinder
	{
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

		readonly Resolver resolver;
		readonly Injector injector;
		readonly Instantiator instantiator;

		public Binder() : this(null) { }

		public Binder(IBinder parent)
		{
			resolver = new Resolver(parent == null ? null : parent.Resolver);
			injector = new Injector(resolver);
			instantiator = new Instantiator(injector, resolver);

			Bind<IBinder>().ToInstance(this);
			Bind<IInjector>().ToInstance(injector);
			Bind<IResolver>().ToInstance(resolver);
			Bind<IInstantiator>().ToInstance(instantiator);
		}

		public IBindingContext Bind(Type contractType)
		{
			return new BindingContext(contractType, this, resolver);
		}

		public IBindingContext<TContract> Bind<TContract>() where TContract : class
		{
			return new BindingContext<TContract>(this, resolver);
		}

		public IBindingContext Bind(Type contractType, params Type[] baseTypes)
		{
			return new MultipleBindingContext(contractType, baseTypes, this, resolver);
		}

		public IBindingContext<TContract> Bind<TContract, TBase>() where TContract : class, TBase
		{
			return new MultipleBindingContext<TContract>(new Type[] { typeof(TBase) }, this, resolver);
		}

		public IBindingContext<TContract> Bind<TContract, TBase1, TBase2>() where TContract : class, TBase1, TBase2
		{
			return new MultipleBindingContext<TContract>(new Type[] { typeof(TBase1), typeof(TBase2) }, this, resolver);
		}

		public IBindingContext<TContract> Bind<TContract, TBase1, TBase2, TBase3>() where TContract : class, TBase1, TBase2, TBase3
		{
			return new MultipleBindingContext<TContract>(new Type[] { typeof(TBase1), typeof(TBase2), typeof(TBase3) }, this, resolver);
		}

		public IBindingContext BindAll(Type contractType)
		{
			return new MultipleBindingContext(contractType, contractType.GetInterfaces(), this, resolver);
		}

		public IBindingContext<TContract> BindAll<TContract>() where TContract : class
		{
			return new MultipleBindingContext<TContract>(typeof(TContract).GetInterfaces(), this, resolver);
		}

		public void Unbind(Type contractType)
		{
			resolver.Unregister(contractType);
		}

		public void Unbind(params Type[] contractTypes)
		{
			for (int i = 0; i < contractTypes.Length; i++)
				resolver.Unregister(contractTypes[i]);
		}

		public void Unbind<TContract>()
		{
			Unbind(typeof(TContract));
		}

		public void UnbindAll(Type contractType)
		{
			Unbind(contractType);
			Unbind(contractType.GetInterfaces());
		}

		public void UnbindAll<TContract>()
		{
			UnbindAll(typeof(TContract));
		}

		public void UnbindAll()
		{
			resolver.UnregisterAll();
		}

		public bool HasBinding(Type contractType)
		{
			return resolver.CanResolve(contractType);
		}

		public bool HasBinding<TContract>()
		{
			return HasBinding(typeof(TContract));
		}

		public void Install(IBindingInstaller installer)
		{
			installer.Install(this);
		}
	}
}