using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using System.Reflection;

namespace Pseudo.Internal.Pool
{
	public class PoolTopContentSetter : IPoolSetter
	{
		readonly IPoolSetter[] setters;

		public PoolTopContentSetter(IPoolSetter[] setters)
		{
			this.setters = setters;
		}

		public void SetValue(object instance)
		{
			if (instance == null)
				return;

			PoolUtility.InitializeFields(instance, setters);
		}

		public override string ToString()
		{
			return string.Format("{0}({1})", GetType().Name, PDebug.ToString(setters));
		}
	}
}