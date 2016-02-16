using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;

namespace Pseudo.Internal.Pool
{
	public class FieldInitializer : IFieldInitializer
	{
		readonly IPoolSetter[] setters;

		public FieldInitializer(object source)
		{
			setters = GetSetters(source, new List<object> { source });
		}

		public void InitializeFields(object instance)
		{
			PoolUtility.InitializeFields(instance, setters);
		}

		static IPoolSetter[] GetSetters(object instance, List<object> toIgnore)
		{
			var type = instance.GetType();
			var allFields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
			var setters = new List<IPoolSetter>(allFields.Length);
			bool isInitializable = instance is IPoolSettersInitializable;

			if (isInitializable)
				((IPoolSettersInitializable)instance).OnPrePoolSettersInitialize();

			for (int i = 0; i < allFields.Length; i++)
			{
				var field = allFields[i];
				var value = field.GetValue(instance);

				if (ShouldInitialize(field, value))
					setters.Add(GetSetter(value, field, toIgnore));
			}

			if (isInitializable)
				((IPoolSettersInitializable)instance).OnPostPoolSettersInitialize(setters);

			return setters.ToArray();
		}

		static IPoolSetter GetSetter(object value, FieldInfo field, List<object> toIgnore)
		{
			if (value == null)
				return new PoolSetter(field, value);
			else if (toIgnore.Contains(value))
				throw new InitializationCycleException(field);

			var copier = CopyUtility.GetCopier(value.GetType());

			if (copier != null)
				return new PoolCopierSetter(copier, field, value);
			else if (value is IList)
				return new PoolArraySetter(field, value.GetType(), GetElementSetters((IList)value, field, toIgnore));
			else if (field.IsDefined(typeof(InitializeContentAttribute), true))
			{
				if (!(value is ValueType))
					toIgnore.Add(value);

				return new PoolContentSetter(field, value.GetType(), GetSetters(value, toIgnore));
			}
			else
				return new PoolSetter(field, value);
		}

		static List<IPoolElementSetter> GetElementSetters(IList array, FieldInfo field, List<object> toIgnore)
		{
			var setters = new List<IPoolElementSetter>(array.Count);

			for (int i = 0; i < array.Count; i++)
				setters.Add(GetElementSetter(array[i], field, toIgnore));

			return setters;
		}

		static IPoolElementSetter GetElementSetter(object element, FieldInfo field, List<object> toIgnore)
		{
			if (element == null)
				return new PoolElementSetter(element);
			else if (toIgnore.Contains(element))
				throw new InitializationCycleException(field);

			var copier = CopyUtility.GetCopier(element.GetType());

			if (copier != null)
				return new PoolElementCopierSetter(copier, element);
			else if (field.IsDefined(typeof(InitializeContentAttribute), true))
			{
				if (!(element is ValueType))
					toIgnore.Add(element);

				return new PoolElementContentSetter(element.GetType(), GetSetters(element, toIgnore));
			}
			else
				return new PoolElementSetter(element);
		}

		static bool ShouldInitialize(FieldInfo field, object value)
		{
			if (value == null)
				return true;
			else if (field.IsDefined(typeof(InitializeValueAttribute), true) || field.IsDefined(typeof(InitializeContentAttribute), true))
				return true;
			else if (field.IsDefined(typeof(DoNotInitializeAttribute), true) || field.DeclaringType.IsDefined(typeof(DoNotInitializeAttribute), true))
				return false;
			else if (field.IsInitOnly || field.IsBackingField())
				return false;
			else if ((value is UnityEngine.Object) && (field.IsPublic || field.IsDefined(typeof(SerializeField), true)) && field.DeclaringType.IsDefined(typeof(SerializableAttribute), true))
				return false;
			else
				return true;
		}
	}
}
