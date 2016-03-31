using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo.Pooling.Internal
{
	public class PoolElementContentSetter : IPoolElementSetter
	{
		readonly Type elementType;
		readonly IPoolSetter[] setters;
		readonly bool isUnityObject;

		public PoolElementContentSetter(Type elementType, IPoolSetter[] setters)
		{
			this.elementType = elementType;
			this.setters = setters;
			isUnityObject = typeof(UnityEngine.Object).IsAssignableFrom(elementType);
		}

		public void SetValue(IList array, int index)
		{
			if (array == null)
				return;

			if (array.Count > index)
			{
				var value = array[index];

				if (value == null)
				{
					if (isUnityObject)
						return;
					else
						value = array[index] = TypePoolManager.Create(elementType);
				}

				PoolUtility.InitializeFields(value, setters);
			}
		}

		public override string ToString()
		{
			return string.Format("{0}({1})", GetType().Name, PDebug.ToString(setters));
		}
	}
}