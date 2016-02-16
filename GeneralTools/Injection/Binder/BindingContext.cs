using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal.Injection
{
	public class BindingContext : IBindingContext
	{
		protected readonly Type contractType;
		protected readonly Binder binder;
		protected readonly Resolver resolver;

		public BindingContext(Type contractType, Binder binder, Resolver resolver)
		{
			this.contractType = contractType;
			this.binder = binder;
			this.resolver = resolver;
		}

		public IBindingCondition ToSingle()
		{
			return ToFactory(new SingletonFactory(contractType, binder.Instantiator));
		}

		public IBindingCondition ToSingle(Type concreteType)
		{
			return ToFactory(new SingletonFactory(concreteType, binder.Instantiator));
		}

		public IBindingCondition ToSinglePrefab(UnityEngine.Object prefab)
		{
			return ToFactory(new SingletonPrefabFactory(prefab, binder.Injector));
		}

		public IBindingCondition ToSinglePrefab(GameObject prefab)
		{
			return ToFactory(new SingletonGameObjectFactory(prefab, binder.Injector));
		}

		public IBindingCondition ToTransient()
		{
			return ToFactory(new TransientFactory(contractType, binder.Instantiator));
		}

		public IBindingCondition ToTransient(Type concreteType)
		{
			return ToFactory(new TransientFactory(concreteType, binder.Instantiator));
		}

		public IBindingCondition ToTransientPrefab(UnityEngine.Object prefab)
		{
			return ToFactory(new TransientPrefabFactory(prefab, binder.Injector));
		}

		public IBindingCondition ToTransientPrefab(GameObject prefab)
		{
			return ToFactory(new TransientGameObjectFactory(prefab, binder.Injector));
		}

		public IBindingCondition ToInstance(object instance)
		{
			return ToFactory(new InstanceFactory(instance));
		}

		public IBindingCondition ToMethod(InjectionMethod<object> method)
		{
			return ToFactory(new MethodFactory(binder, method));
		}

		public IBindingCondition ToFactory(IFactory factory)
		{
			var data = new FactoryData
			{
				Factory = factory,
				Conditions = new List<Predicate<InjectionContext>>()
			};

			resolver.Register(contractType, data);

			return new BindingCondition(data);
		}
	}

	public class BindingContext<TContract> : BindingContext, IBindingContext<TContract>
	{
		public BindingContext(Binder binder, Resolver resolver) : base(typeof(TContract), binder, resolver) { }

		public IBindingCondition ToSingle<TConcrete>() where TConcrete : class, TContract
		{
			return base.ToSingle(typeof(TConcrete));
		}

		public IBindingCondition ToSinglePrefab<TConcrete>(TConcrete prefab) where TConcrete : UnityEngine.Object, TContract
		{
			return base.ToSinglePrefab(prefab);
		}

		public IBindingCondition ToTransient<TConcrete>() where TConcrete : class, TContract
		{
			return base.ToTransient(typeof(TConcrete));
		}

		public IBindingCondition ToTransientPrefab<TConcrete>(TConcrete prefab) where TConcrete : UnityEngine.Object, TContract
		{
			return base.ToTransientPrefab(prefab);
		}

		public IBindingCondition ToInstance<TConcrete>(TConcrete instance) where TConcrete : class, TContract
		{
			return base.ToInstance(instance);
		}

		public IBindingCondition ToMethod<TConcrete>(InjectionMethod<TConcrete> method) where TConcrete : class, TContract
		{
			return base.ToMethod(method);
		}

		public IBindingCondition ToFactory<TConcrete>(IFactory<TConcrete> factory) where TConcrete : class, TContract
		{
			return base.ToFactory(factory);
		}
	}
}
