using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using UnityEngine.Assertions;

namespace Pseudo.Injection.Internal
{
	public abstract class BindingContextBase : IBindingContext
	{
		public IContainer Container
		{
			get { return container; }
		}
		public Type ContractType
		{
			get { return contractType; }
		}

		protected readonly Type contractType;
		protected readonly IContainer container;

		protected BindingContextBase(Type contractType, IContainer container)
		{
			this.contractType = contractType;
			this.container = container;
		}

		public IBindingCondition ToSingleton()
		{
			return ToSingleton(contractType);
		}

		public IBindingCondition ToSingleton(Type concreteType)
		{
			Assert.IsNotNull(concreteType);
			Assert.IsTrue(concreteType.IsConcrete());
			Assert.IsTrue(concreteType.Is(contractType));

			// Do not use Container.Get because it could cause an infinite loop.
			return ToSingletonMethod(c => c.Container.Instantiator.Instantiate(concreteType));
		}

		public IBindingCondition ToSingletonPrefab(UnityEngine.Object prefab)
		{
			Assert.IsNotNull(prefab);
			Assert.IsTrue(prefab.GetType().Is(contractType));

			return ToSingletonMethod(c =>
			{
				var instance = UnityEngine.Object.Instantiate(prefab);
				c.Container.Injector.Inject(instance);

				return instance;
			});
		}

		public IBindingCondition ToSingletonPrefab(GameObject prefab)
		{
			Assert.IsNotNull(prefab);
			Assert.IsTrue(contractType.Is<GameObject>() || prefab.GetComponent(contractType) != null);

			return ToSingletonMethod(c =>
			{
				var instance = UnityEngine.Object.Instantiate(prefab);
				var components = instance.GetComponentsInChildren<MonoBehaviour>();

				for (int i = 0; i < components.Length; i++)
					c.Container.Injector.Inject(components[i]);

				if (contractType.Is<GameObject>())
					return instance;
				else
					return instance.GetComponent(contractType);
			});
		}

		public IBindingCondition ToSingletonMethod(InjectionMethod<object> method)
		{
			Assert.IsNotNull(method);

			return ToFactory(new SingletonMethodFactory<object>(method));
		}

		public IBindingCondition ToTransient()
		{
			return ToTransient(contractType);
		}

		public IBindingCondition ToTransient(Type concreteType)
		{
			Assert.IsNotNull(concreteType);
			Assert.IsTrue(concreteType.IsConcrete());
			Assert.IsTrue(concreteType.Is(contractType));

			// Do not use Container.Get because it could cause an infinite loop.
			return ToTransientMethod(c => c.Container.Instantiator.Instantiate(concreteType));
		}

		public IBindingCondition ToTransientPrefab(UnityEngine.Object prefab)
		{
			Assert.IsNotNull(prefab);
			Assert.IsTrue(prefab.GetType().Is(contractType));

			return ToTransientMethod(c =>
			{
				var instance = UnityEngine.Object.Instantiate(prefab);
				c.Container.Injector.Inject(instance);

				return instance;
			});
		}

		public IBindingCondition ToTransientPrefab(GameObject prefab)
		{
			Assert.IsNotNull(prefab);
			Assert.IsTrue(contractType.Is<GameObject>() || prefab.GetComponent(contractType) != null);

			return ToTransientMethod(c =>
			{
				var instance = UnityEngine.Object.Instantiate(prefab);
				var components = instance.GetComponentsInChildren<MonoBehaviour>();

				for (int i = 0; i < components.Length; i++)
					c.Container.Injector.Inject(components[i]);

				if (contractType.Is<GameObject>())
					return instance;
				else
					return instance.GetComponent(contractType);
			});
		}

		public IBindingCondition ToTransientMethod(InjectionMethod<object> method)
		{
			Assert.IsNotNull(method);

			return ToFactory(new TransientMethodFactory<object>(method));
		}

		public IBindingCondition ToInstance(object instance)
		{
			Assert.IsTrue(instance == null || instance.GetType().Is(contractType));

			return ToSingletonMethod(c => instance);
		}

		public IBindingCondition ToFactory(Type factoryType)
		{
			Assert.IsNotNull(factoryType);
			Assert.IsTrue(factoryType.Is<IInjectionFactory>());

			container.Binder.Bind(factoryType).ToSingleton();

			return ToTransientMethod(c => ((IInjectionFactory)c.Container.Resolver.Resolve(factoryType)).Create(c));
		}

		public abstract IBindingCondition ToFactory(IInjectionFactory factory);
	}

	public abstract class BindingContextBase<TContract> : BindingContextBase, IBindingContext<TContract>
	{
		protected BindingContextBase(IContainer container) : base(typeof(TContract), container) { }

		public IBindingCondition ToSingleton<TConcrete>() where TConcrete : TContract
		{
			return base.ToSingleton(typeof(TConcrete));
		}

		public IBindingCondition ToSingletonPrefab<TConcrete>(TConcrete prefab) where TConcrete : UnityEngine.Object, TContract
		{
			return base.ToSingletonPrefab(prefab);
		}

		public IBindingCondition ToSingletonMethod<TConcrete>(InjectionMethod<TConcrete> method) where TConcrete : TContract
		{
			Assert.IsNotNull(method);

			return ToFactory(new SingletonMethodFactory<TConcrete>(method));
		}

		public IBindingCondition ToTransient<TConcrete>() where TConcrete : TContract
		{
			return base.ToTransient(typeof(TConcrete));
		}

		public IBindingCondition ToTransientPrefab<TConcrete>(TConcrete prefab) where TConcrete : UnityEngine.Object, TContract
		{
			return base.ToTransientPrefab(prefab);
		}

		public IBindingCondition ToTransientMethod<TConcrete>(InjectionMethod<TConcrete> method) where TConcrete : TContract
		{
			Assert.IsNotNull(method);

			return ToFactory(new TransientMethodFactory<TConcrete>(method));
		}

		public IBindingCondition ToInstance<TConcrete>(TConcrete instance) where TConcrete : TContract
		{
			return base.ToInstance(instance);
		}

		public IBindingCondition ToFactory<TFactory>() where TFactory : IInjectionFactory<TContract>
		{
			return base.ToFactory(typeof(TFactory));
		}

		public IBindingCondition ToFactory(IInjectionFactory<TContract> factory)
		{
			return ToFactory((IInjectionFactory)factory);
		}
	}
}
