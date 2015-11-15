using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using System.Reflection;
using Pseudo.Internal;

namespace Pseudo.Internal
{
	public static class PoolUtility
	{
		public static List<IPoolFieldData> GetFieldData(object instance)
		{
			Type type = instance.GetType();
			FieldInfo[] allFields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
			List<IPoolFieldData> fields = new List<IPoolFieldData>(allFields.Length);

			for (int i = 0; i < allFields.Length; i++)
			{
				FieldInfo field = allFields[i];
				object value = field.GetValue(instance);

				if (!field.IsInitOnly && !field.IsDefined(typeof(DoNotInitializeAttribute), true) && !field.IsBackingField())
				{
					if (value != null && field.IsDefined(typeof(InitializeContentAttribute), true))
						fields.Add(new PoolFieldContentData(field, GetFieldData(value)));
					else
						fields.Add(new PoolFieldData(field, value));
				}
			}

			return fields;
		}

		public static void InitializeFields(object instance, List<IPoolFieldData> fields)
		{
			for (int i = 0; i < fields.Count; i++)
				fields[i].SetValue(instance);
		}
	}
}