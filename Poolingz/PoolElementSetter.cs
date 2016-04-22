using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo.Pooling.Internal
{
	public class PoolElementSetter : IPoolElementSetter
	{
		readonly object value;

		public PoolElementSetter(object value)
		{
			this.value = value;
		}

		public void SetValue(IList array, int index)
		{
			if (array == null)
				return;

			if (array.Count > index)
				array[index] = value;
		}

		public override string ToString()
		{
			return string.Format("{0}({1})", GetType().Name, PDebug.ToString(value));
		}
	}
}