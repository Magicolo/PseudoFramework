using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal.Injection
{
	public class MultipleBindingContext : BindingContextBase
	{
		readonly Type[] baseTypes;

		public MultipleBindingContext(Type contractType, Type[] baseTypes, Binder binder, Resolver resolver) : base(contractType, binder, resolver)
		{
			this.baseTypes = baseTypes;
		}

		public override IBindingCondition ToFactory(IInjectionFactory factory)
		{
			var conditions = new IBindingCondition[baseTypes.Length + 1];

			for (int i = 0; i < baseTypes.Length; i++)
				conditions[i] = binder.Bind(baseTypes[i]).ToFactory(factory);

			conditions[conditions.Length - 1] = binder.Bind(contractType).ToFactory(factory);

			return new MultipleBindingCondition(conditions);
		}
	}

	public class MultipleBindingContext<TContract> : BindingContextBase<TContract>
	{
		readonly Type[] baseTypes;

		public MultipleBindingContext(Type[] baseTypes, Binder binder, Resolver resolver) : base(binder, resolver)
		{
			this.baseTypes = baseTypes;
		}

		public override IBindingCondition ToFactory(IInjectionFactory factory)
		{
			return binder.Bind(contractType, baseTypes).ToFactory(factory);
		}
	}
}
