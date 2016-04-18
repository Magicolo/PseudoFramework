using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using UnityEngine.Assertions;

namespace Pseudo.Injection.Internal
{
	public class BindingContract : IBindingContract
	{
		public Type ContractType { get; private set; }
		public Type[] BaseTypes { get; private set; }
		public IContainer Container { get; private set; }

		public BindingContract(Type contractType, IContainer container)
			: this(contractType, Type.EmptyTypes, container) { }

		public BindingContract(Type contractType, Type[] baseTypes, IContainer container)
		{
			ContractType = contractType;
			BaseTypes = baseTypes;
			Container = container;
		}

		public IBindingScope ToSelf()
		{
			Assert.IsTrue(ContractType.IsConcrete());

			return To(ContractType);
		}

		public IBindingScope To(Type concreteType)
		{
			Assert.IsNotNull(concreteType);
			Assert.IsTrue(concreteType.IsConcrete());
			Assert.IsTrue(concreteType.Is(ContractType));

			return ToMethod(c => c.Container.Instantiator
				.Instantiate(new InjectionContext { Container = c.Container, DeclaringType = concreteType }));
		}

		public IBindingCondition ToInstance(object instance)
		{
			Assert.IsNotNull(instance);
			Assert.IsTrue(instance.GetType().Is(ContractType));

			return ToMethod(c => instance).AsSingleton();
		}

		public IBindingScope ToMethod(InjectionMethod<object> method)
		{
			Assert.IsNotNull(method);

			return ToFactory(new MethodFactory<object>(method));
		}

		public IBindingScope ToFactory(Type factoryType)
		{
			Assert.IsNotNull(factoryType);
			Assert.IsTrue(factoryType.Is<IInjectionFactory>());

			Container.Binder.Bind(factoryType).ToSelf().AsSingleton();

			return ToMethod(c => ((IInjectionFactory)c.Container.Resolver
				.Resolve(new InjectionContext { Container = c.Container, ContractType = factoryType }))
				.Create(c));
		}

		public IBindingScope ToFactory(IInjectionFactory factory)
		{
			var binding = new Binding(ContractType, BaseTypes, factory);
			Container.Binder.Bind(binding);

			return new BindingScope(binding);
		}
	}

	public class BindingContract<TContract> : BindingContract, IBindingContract<TContract>
	{
		public BindingContract(IContainer container)
			: this(Type.EmptyTypes, container) { }

		public BindingContract(Type[] baseTypes, IContainer container) : base(typeof(TContract), baseTypes, container) { }

		public IBindingScope To<TConcrete>() where TConcrete : TContract
		{
			return base.To(typeof(TConcrete));
		}

		public IBindingCondition ToInstance<TConcrete>(TConcrete instance) where TConcrete : TContract
		{
			return base.ToInstance(instance);
		}

		public IBindingScope ToMethod<TConcrete>(InjectionMethod<TConcrete> method) where TConcrete : TContract
		{
			return base.ToFactory(new MethodFactory<TConcrete>(method));
		}

		public IBindingScope ToFactory<TFactory>() where TFactory : IInjectionFactory<TContract>
		{
			return base.ToFactory(typeof(TFactory));
		}

		public IBindingScope ToFactory<TConcrete>(IInjectionFactory<TConcrete> factory) where TConcrete : TContract
		{
			return base.ToFactory(factory);
		}
	}
}
