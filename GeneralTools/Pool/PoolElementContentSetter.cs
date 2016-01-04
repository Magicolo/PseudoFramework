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
		readonly Type type;
		readonly List<IPoolSetter> setters;
		readonly bool isUnityObject;

		public PoolElementContentSetter(Type type, List<IPoolSetter> setters)
		{
			this.type = type;
			this.setters = setters;
			isUnityObject = typeof(UnityEngine.Object).IsAssignableFrom(type);
		}

		public void SetValue(IList array, int index)
		{
			if (array.Count > index)
			{
				var value = array[index];

				if (value == null)
				{
					if (isUnityObject)
						return;
					else
						value = (array[index] = TypePoolManager.Create(type));
				}

				for (int i = 0; i < setters.Count; i++)
					setters[i].SetValue(value);
			}
		}

		public override string ToString()
		{
			return string.Format("{0}({1})", GetType().Name, PDebug.ToString(setters));
		}
	}
}