using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Injection.Internal
{
	public class SingletonMethodFactory<TConcrete> : MethodFactoryBase<TConcrete>
	{
		TConcrete instance;
		bool isCreated;

		public SingletonMethodFactory(InjectionMethod<TConcrete> method) : base(method) { }

		public override TConcrete Create(InjectionContext context)
		{
			if (!isCreated)
			{
				isCreated = true;
				instance = method(context);
			}

			return instance;
		}
	}
}
