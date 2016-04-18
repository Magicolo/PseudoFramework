using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Injection.Internal
{
	public class MethodFactory<TConcrete> : IInjectionFactory<TConcrete>
	{
		readonly InjectionMethod<TConcrete> method;

		public MethodFactory(InjectionMethod<TConcrete> method)
		{
			this.method = method;
		}

		public TConcrete Create(InjectionContext context)
		{
			return method(context);
		}

		object IInjectionFactory.Create(InjectionContext context)
		{
			return Create(context);
		}
	}
}
