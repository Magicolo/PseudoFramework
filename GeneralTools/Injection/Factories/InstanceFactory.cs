using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal.Injection
{
	public class InstanceFactory : IFactory
	{
		readonly object instance;

		public InstanceFactory(object instance)
		{
			this.instance = instance;
		}

		public object Create(params object[] arguments)
		{
			return instance;
		}
	}
}
