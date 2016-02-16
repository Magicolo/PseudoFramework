using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal.Injection
{
	public class SingletonFactory : IFactory
	{
		readonly Type concreteType;
		readonly IInstantiator instantiator;
		object instance;

		public SingletonFactory(Type concreteType, IInstantiator instantiator)
		{
			this.concreteType = concreteType;
			this.instantiator = instantiator;
		}

		public object Create(params object[] arguments)
		{
			return instance ?? (instance = instantiator.Instantiate(concreteType, arguments));
		}
	}
}
