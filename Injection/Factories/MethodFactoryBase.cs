using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Injection.Internal
{
	public abstract class MethodFactoryBase<TConcrete> : IInjectionFactory<TConcrete>
	{
		protected readonly InjectionMethod<TConcrete> method;

		protected MethodFactoryBase(InjectionMethod<TConcrete> method)
		{
			this.method = method;
		}

		public abstract TConcrete Create(InjectionContext context);

		object IInjectionFactory.Create(InjectionContext context)
		{
			return Create(context);
		}
	}
}
