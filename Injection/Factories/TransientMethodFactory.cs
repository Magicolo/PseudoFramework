using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Injection.Internal
{
	public class TransientMethodFactory<TConcrete> : MethodFactoryBase<TConcrete>
	{
		public TransientMethodFactory(InjectionMethod<TConcrete> method) : base(method) { }

		public override TConcrete Create(InjectionContext context)
		{
			return method(context);
		}
	}
}
