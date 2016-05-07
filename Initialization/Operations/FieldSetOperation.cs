using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Reflection;

namespace Pseudo.Initialization.Internal
{
	public class FieldSetOperation : IInitializationOperation
	{
		readonly IFieldWrapper field;
		readonly object referenceValue;
		readonly Type type;
		readonly bool isValue;
		readonly bool isPure;
		readonly IInitializationOperation[] operations;

		public FieldSetOperation(IFieldWrapper field, object reference)
		{
			this.field = field;

			referenceValue = field.Get(ref reference);
			type = GetType(referenceValue);
			isValue = type == null ? false : type.IsValueType;
			isPure = type == null || type.IsPure();
			operations = isPure ? null : InitializationUtility.GetInitializer(type).CreateOperations(referenceValue);
		}

		public void Initialize(ref object instance, HashSet<object> toIgnore)
		{
			if (isPure)
			{
				field.Set(ref instance, referenceValue);
				return;
			}

			var instanceValue = field.Get(ref instance);

			if (GetType(instanceValue) != type)
				field.Set(ref instance, referenceValue);
			else if (isValue || toIgnore.Add(instanceValue))
			{
				for (int i = 0; i < operations.Length; i++)
					operations[i].Initialize(ref instanceValue, toIgnore);

				field.Set(ref instance, instanceValue);
			}
		}

		Type GetType(object value)
		{
			return value is ValueType || value != null ? value.GetType() : null;
		}
	}
}
