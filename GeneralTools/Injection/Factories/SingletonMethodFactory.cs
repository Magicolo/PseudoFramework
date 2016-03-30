using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal.Injection
{
	public class SingletonMethodFactory<TConcrete> : MethodFactoryBase<TConcrete>
	{
		object instance;

		public SingletonMethodFactory(Type contractType, IBinder binder, InjectionMethod<TConcrete> method)
			: base(contractType, binder, method)
		{ }

		public override object Create(InjectionContext argument)
		{
			if (instance == null)
				instance = method(argument);

			return instance;
		}
	}
}
