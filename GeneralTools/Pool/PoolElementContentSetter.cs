using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo.Internal.Pool
{
	public class PoolElementContentSetter : IPoolElementSetter
	{
		readonly List<IPoolSetter> setters;

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

		public override string ToString()
		{
			return string.Format("{0}({1})", GetType().Name, PDebug.ToString(setters));
		}
	}
}