using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using UnityEngine.Assertions;

namespace Pseudo.Internal.Injection
{
	public class BindingContext : BindingContextBase
	{
		public BindingContext(Type contractType, Binder binder, Resolver resolver) : base(contractType, binder, resolver) { }

		public override IBindingCondition ToFactory(IInjectionFactory factory)
		{
			Assert.IsNotNull(factory);

			var data = new FactoryData(factory);
			resolver.Register(contractType, data);

			return new BindingCondition(data);
		}
	}

	public class BindingContext<TContract> : BindingContextBase<TContract>, IBindingContext<TContract> where TContract : class
	{
		public BindingContext(Binder binder, Resolver resolver) : base(binder, resolver) { }

		public override IBindingCondition ToFactory(IInjectionFactory factory)
		{
			return binder.Bind(contractType).ToFactory(factory);
		}
	}
}