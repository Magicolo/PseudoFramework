using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo.Internal
{
	public class PoolElementSetter : IPoolElementSetter
	{
		object value;

		public PoolElementSetter(object value)
		{
			this.value = value;
		}

		public void SetValue(IList array, int index)
		{
			if (array.Count > index)
				array[index] = value;
		}
	}
}