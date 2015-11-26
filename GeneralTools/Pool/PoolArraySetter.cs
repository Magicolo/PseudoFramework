using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using System.Reflection;

namespace Pseudo.Internal
{
	public class PoolArraySetter : IPoolSetter
	{
		List<IPoolElementSetter> setters;
		FieldInfo field;

		public PoolArraySetter(List<IPoolElementSetter> setters, FieldInfo field)
		{
			this.setters = setters;
			this.field = field;
		}

		public void SetValue(object instance)
		{
			if (instance == null)
				return;

			if (instance.GetType() != field.DeclaringType)
				throw new TypeMismatchException(string.Format("Instance type {0} doesn't match {1}.", instance.GetType().Name, field.DeclaringType.Name));

			var array = (IList)field.GetValue(instance);

			if (array == null)
			{
				if (field.FieldType.IsArray)
					array = Array.CreateInstance(field.FieldType.GetElementType(), setters.Count);
				else
					array = (IList)Activator.CreateInstance(field.FieldType);

				field.SetValue(instance, array);
			}

			if (array.Count != setters.Count)
			{
				if (field.FieldType.IsArray)
				{
					array = Array.CreateInstance(field.FieldType.GetElementType(), setters.Count);
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
	}
}