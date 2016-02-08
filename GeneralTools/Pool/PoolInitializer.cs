using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal.Pool
{
	public class PoolInitializer : IPoolInitializer
	{
		readonly IPoolSetter[] setters;

		public PoolInitializer(IPoolSetter[] setters)
		{
			this.setters = setters;
		}

		public void Initialize(object instance)
		{
			PoolUtility.InitializeFields(instance, setters);
		}
	}
}
