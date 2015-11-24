using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using System.Reflection;
using Pseudo.Internal;
using System.Runtime.Serialization;

namespace Pseudo.Internal
{
	public static class PoolUtility
	{
		static readonly Dictionary<Type, object> defaultValues = new Dictionary<Type, object>();

		public static List<IPoolFieldData> GetFieldData(object instance)
		{
			Type type = instance.GetType();
			FieldInfo[] allFields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
			List<IPoolFieldData> fields = new List<IPoolFieldData>(allFields.Length);

			for (int i = 0; i < allFields.Length; i++)
			{
				FieldInfo field = allFields[i];
				object value = field.GetValue(instance);
				bool isDefault = IsDefault(value);

				if (!field.IsInitOnly && !field.IsDefined(typeof(DoNotInitializeAttribute), true) && !field.IsBackingField())
				{
					if (!isDefault && field.IsDefined(typeof(InitializeContentAttribute), true))
						fields.Add(new PoolFieldContentData(field, value, isDefault, GetFieldData(value)));
					else
						fields.Add(new PoolFieldData(field, value, isDefault));
				}
			}

			return fields;
		}

		public static void InitializeFields(object instance, bool initializeDefaults, List<IPoolFieldData> fields)
		{
			for (int i = 0; i < fields.Count; i++)
				fields[i].SetValue(instance, initializeDefaults);
		}

		public static object GetDefault(Type type)
		{
			object defaultValue = null;

			if (type.IsValueType)
			{
				if (!defaultValues.TryGetValue(type, out defaultValue))
				{
					defaultValue = FormatterServices.GetUninitializedObject(type);
					defaultValues[type] = defaultValue;
				}
			}

			return defaultValue;
		}

		public static bool IsDefault(object value)
		{
			return value == null || Equals(value, GetDefault(value.GetType()));
		}
	}
}