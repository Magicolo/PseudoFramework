using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Reflection;

namespace Pseudo.Initialization.Internal
{
	public class ArraySetOperation : IInitializationOperation
	{
		readonly int index;
		readonly object referenceValue;
		readonly Type type;
		readonly bool isValue;
		readonly bool isPure;
		readonly IInitializationOperation[] operations;

		public ArraySetOperation(int index, object reference)
		{
			this.index = index;

			referenceValue = ((IList)reference)[index];
			type = GetType(referenceValue);
			isValue = type == null ? false : type.IsValueType;
			isPure = type == null || type.IsPureValueType();
			operations = isPure ? null : InitializationUtility.GetInitializer(type).CreateOperations(referenceValue);
		}

		public void Initialize(ref object instance, HashSet<object> toIgnore)
		{
			var list = instance as IList;

			if (isPure)
			{
				list[index] = referenceValue;
				return;
			}

			var instanceValue = list[index];

			if (GetType(instanceValue) != type)
				list[index] = referenceValue;
			else if (isValue || toIgnore.Add(instanceValue))
			{
				for (int i = 0; i < operations.Length; i++)
					operations[i].Initialize(ref instanceValue, toIgnore);

				list[index] = instanceValue;
			}
		}

		Type GetType(object value)
		{
			return value is ValueType || value != null ? value.GetType() : null;
		}
	}
}
