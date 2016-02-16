using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal.Injection
{
	public class TransientFactory : IFactory
	{
		readonly Type concreteType;
		readonly IInstantiator instantiator;

		public TransientFactory(Type concreteType, IInstantiator instantiator)
		{
			this.concreteType = concreteType;
			this.instantiator = instantiator;
		}

		public object Create(params object[] arguments)
		{
			return instantiator.Instantiate(concreteType, arguments);
		}
	}
}
