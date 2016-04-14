using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Injection.Internal
{
	public class MultipleBindingContext : BindingContextBase
	{
		readonly Type[] baseTypes;

		public MultipleBindingContext(Type contractType, Type[] baseTypes, IContainer container) : base(contractType, container)
		{
			this.baseTypes = baseTypes;
		}

		public override IBindingCondition ToFactory(IInjectionFactory factory)
		{
			return new MultipleBindingCondition(baseTypes
				.Joined(contractType)
				.Select(t => container.Binder.Bind(t).ToFactory(factory))
				.ToArray());
		}
	}

	public class MultipleBindingContext<TContract> : BindingContextBase<TContract>
	{
		readonly Type[] baseTypes;

		public MultipleBindingContext(Type[] baseTypes, IContainer container) : base(container)
		{
			this.baseTypes = baseTypes;
		}

		public override IBindingCondition ToFactory(IInjectionFactory factory)
		{
			return container.Binder.Bind(contractType, baseTypes).ToFactory(factory);
		}
	}
}
