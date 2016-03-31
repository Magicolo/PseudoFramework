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
		public TransientMethodFactory(Type contractType, IBinder binder, InjectionMethod<TConcrete> method)
			: base(contractType, binder, method)
		{ }

		public override object Create(InjectionContext argument)
		{
			return method(argument);
		}
	}
}
