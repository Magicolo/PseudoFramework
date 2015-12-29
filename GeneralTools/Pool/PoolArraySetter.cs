using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using System.Reflection;

namespace Pseudo.Internal.Pool
{
	public class PoolArraySetter : IPoolSetter
	{
		readonly FieldInfo field;
		readonly Type type;
		readonly List<IPoolElementSetter> setters;

		public PoolArraySetter(FieldInfo field, Type type, List<IPoolElementSetter> setters)
		{
			this.field = field;
			this.type = type;
			this.setters = setters;
		}

		public void SetValue(object instance)
		{
			if (instance == null)
				return;

			var array = (IList)field.GetValue(instance);

			if (array == null)
			{
				if (type.IsArray)
					array = Array.CreateInstance(type.GetElementType(), setters.Count);
				else
					array = (IList)Activator.CreateInstance(type);

				field.SetValue(instance, array);
			}

			if (array.Count != setters.Count)
			{
				if (type.IsArray)
				{
					array = Array.CreateInstance(type.GetElementType(), setters.Count);
					field.SetValue(instance, array);
				}
				else if (!array.IsFixedSize)
					PoolUtility.Resize(array, setters.Count);
				else
					return;
			}

			for (int i = 0; i < setters.Count; i++)
				setters[i].SetValue(array, i);
		}

		public override string ToString()
		{
			return string.Format("{0}({1}, {2}, {3})", GetType().Name, field.Name, field.FieldType.Name, PDebug.ToString(setters));
		}
	}
}