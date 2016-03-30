using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;
using Pseudo.Internal.Copy;

namespace Pseudo.Internal.Pool
{
	public class FieldInitializer : IFieldInitializer
	{
		readonly IPoolSetter[] setters;

		public FieldInitializer(object source)
		{
			bool isInitializable = source is IPoolFieldsInitializable;

			if (isInitializable)
				((IPoolFieldsInitializable)source).OnPrePoolFieldsInitialize(this);

			setters = GetSetters(source, new List<object> { source });

			if (isInitializable)
				((IPoolFieldsInitializable)source).OnPostPoolFieldsInitialize(this);
		}

		public void InitializeFields(object instance)
		{
			PoolUtility.InitializeFields(instance, setters);
		}

		static IPoolSetter[] GetSetters(object instance, List<object> toIgnore)
		{
			var type = instance.GetType();
			var fields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
				.Concat(TypeUtility.GetBaseTypes(type) // Need to recover the private members from base types.
				.SelectMany(t => t.GetFields(BindingFlags.Instance | BindingFlags.NonPublic))
				.Where(f => f.IsPrivate))
				.ToArray();
			var setters = new List<IPoolSetter>(fields.Length);

			for (int i = 0; i < fields.Length; i++)
			{
				var field = fields[i];
				var value = field.GetValue(instance);

				if (ShouldInitialize(field, value))
					setters.Add(GetSetter(value, field, toIgnore));
			}

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
