using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo.Internal
{
	public class PoolElementContentSetter : IPoolElementSetter
	{
		List<IPoolSetter> setters;

		public PoolElementContentSetter(List<IPoolSetter> setters)
		{
			this.setters = setters;
		}

		public void SetValue(IList array, int index)
		{
			if (array.Count > index)
			{
				var value = array[index];

				if (value != null)
				{
					for (int i = 0; i < setters.Count; i++)
						setters[i].SetValue(value);
				}
			}
		}
	}
}