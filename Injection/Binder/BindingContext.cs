using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using UnityEngine.Assertions;

namespace Pseudo.Injection.Internal
{
	public class BindingContext : BindingContextBase
	{
		public BindingContext(Type contractType, IContainer container) : base(contractType, container) { }

		public override IBindingCondition ToFactory(IInjectionFactory factory)
		{
			Assert.IsNotNull(factory);

			var binding = new Binding(contractType, factory);
			container.Binder.Bind(binding);

			return new BindingCondition(binding);
		}
	}

	public class BindingContext<TContract> : BindingContextBase<TContract>, IBindingContext<TContract>
	{
		public BindingContext(IContainer container) : base(container) { }

		public override IBindingCondition ToFactory(IInjectionFactory factory)
		{
			return container.Binder.Bind(contractType).ToFactory(factory);
		}
	}
}