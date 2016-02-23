using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using UnityEngine.Assertions;

namespace Pseudo.Internal.Injection
{
	public abstract class BindingContextBase : IBindingContext
	{
		protected readonly Type contractType;
		protected readonly Binder binder;
		protected readonly Resolver resolver;

		protected BindingContextBase(Type contractType, Binder binder, Resolver resolver)
		{
			this.contractType = contractType;
			this.binder = binder;
			this.resolver = resolver;
		}

		public virtual IBindingCondition ToSingle()
		{
			return ToSingle(contractType);
		}

		public virtual IBindingCondition ToSingle(Type concreteType)
		{
			Assert.IsNotNull(concreteType);
			Assert.IsTrue(concreteType.IsClass && !concreteType.IsAbstract);

			return ToSingleMethod(c => c.Binder.Instantiator.Instantiate(concreteType));
		}

		public virtual IBindingCondition ToSinglePrefab(UnityEngine.Object prefab)
		{
			Assert.IsNotNull(prefab);

			return ToSingleMethod(c =>
			{
				var instance = UnityEngine.Object.Instantiate(prefab);
				c.Binder.Injector.Inject(instance);

				return instance;
			});
		}

		public virtual IBindingCondition ToSinglePrefab(GameObject prefab)
		{
			Assert.IsNotNull(prefab);

			return ToSingleMethod(c =>
			{
				var instance = UnityEngine.Object.Instantiate(prefab);
				c.Binder.Injector.Inject(instance, true);

				return instance;
			});
		}

		public virtual IBindingCondition ToSingleMethod(InjectionMethod<object> method)
		{
			Assert.IsNotNull(method);

			return ToFactory(new SingletonMethodFactory(contractType, binder, method));
		}

		public virtual IBindingCondition ToTransient()
		{
			return ToTransient(contractType);
		}

		public virtual IBindingCondition ToTransient(Type concreteType)
		{
			Assert.IsNotNull(concreteType);
			Assert.IsTrue(concreteType.IsClass && !concreteType.IsAbstract);

			return ToTransientMethod(c => c.Binder.Instantiator.Instantiate(concreteType));
		}

		public virtual IBindingCondition ToTransientPrefab(UnityEngine.Object prefab)
		{
			Assert.IsNotNull(prefab);

			return ToTransientMethod(c =>
			{
				var instance = UnityEngine.Object.Instantiate(prefab);
				c.Binder.Injector.Inject(instance);

				return instance;
			});
		}

		public virtual IBindingCondition ToTransientPrefab(GameObject prefab)
		{
			Assert.IsNotNull(prefab);

			return ToTransientMethod(c =>
			{
				var instance = UnityEngine.Object.Instantiate(prefab);
				c.Binder.Injector.Inject(instance, true);

				return instance;
			});
		}

		public virtual IBindingCondition ToTransientMethod(InjectionMethod<object> method)
		{
			Assert.IsNotNull(method);

			return ToFactory(new TransientMethodFactory(contractType, binder, method));
		}

		public virtual IBindingCondition ToInstance(object instance)
		{
			return ToSingleMethod(c => instance);
		}

		public virtual IBindingCondition ToFactory(Type factoryType)
		{
			Assert.IsNotNull(factoryType);
			Assert.IsTrue(typeof(IFactory).IsAssignableFrom(factoryType));

			binder.Bind(factoryType).ToSingle();

			return ToTransientMethod(c => ((IFactory)c.Binder.Resolver.Resolve(factoryType)).Create());

		}

		public virtual IBindingCondition ToFactory(IFactory factory)
		{
			Assert.IsNotNull(factory);

			return ToTransientMethod(c => factory.Create());
		}

		public abstract IBindingCondition ToFactory(IInjectionFactory factory);
	}

	public abstract class BindingContextBase<TContract> : BindingContextBase, IBindingContext<TContract> where TContract : class
	{
		protected BindingContextBase(Binder binder, Resolver resolver) : base(typeof(TContract), binder, resolver) { }

		public virtual IBindingCondition ToSingle<TConcrete>() where TConcrete : class, TContract
		{
			return base.ToSingle(typeof(TConcrete));
		}

		public virtual IBindingCondition ToSinglePrefab<TConcrete>(TConcrete prefab) where TConcrete : UnityEngine.Object, TContract
		{
			return base.ToSinglePrefab(prefab);
		}

		public virtual IBindingCondition ToSingleMethod<TConcrete>(InjectionMethod<TConcrete> method) where TConcrete : class, TContract
		{
			return binder.Bind(contractType).ToSingleMethod(method);
		}

		public virtual IBindingCondition ToTransient<TConcrete>() where TConcrete : class, TContract
		{
			return base.ToTransient(typeof(TConcrete));
		}

		public virtual IBindingCondition ToTransientPrefab<TConcrete>(TConcrete prefab) where TConcrete : UnityEngine.Object, TContract
		{
			return base.ToTransientPrefab(prefab);
		}

		public virtual IBindingCondition ToTransientMethod<TConcrete>(InjectionMethod<TConcrete> method) where TConcrete : class, TContract
		{
			return binder.Bind(contractType).ToTransientMethod(method);
		}

		public virtual IBindingCondition ToInstance<TConcrete>(TConcrete instance) where TConcrete : class, TContract
		{
			return base.ToInstance(instance);
		}

		public virtual IBindingCondition ToFactory<TFactory>() where TFactory : class, IFactory<TContract>
		{
			binder.Bind<TFactory>().ToSingle();

			return ToTransientMethod(c => c.Binder.Resolver.Resolve<TFactory>().Create());
		}

		public virtual IBindingCondition ToFactory(IFactory<TContract> factory)
		{
			Assert.IsNotNull(factory);

			return ToTransientMethod(c => factory.Create());
		}
	}
}
