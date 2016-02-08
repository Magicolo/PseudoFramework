using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;
using Pseudo.Internal;

namespace Pseudo
{
	public static class TypeUtility
	{
		static readonly Dictionary<Type, Type[]> typeToAssignableTypes = new Dictionary<Type, Type[]>();
		static readonly Dictionary<Type, Type[]> typeToSubclassTypes = new Dictionary<Type, Type[]>();
		static readonly Dictionary<Type, Type[]> typeToDefinedTypes = new Dictionary<Type, Type[]>();
		static readonly Dictionary<Type, FieldInfo[]> typeToFields = new Dictionary<Type, FieldInfo[]>();
		static readonly Dictionary<string, Type> typeNameToType = new Dictionary<string, Type>();
		static readonly Dictionary<Type, object> typeToDefaultValue = new Dictionary<Type, object>();

		static Type[] allTypes;
		public static Type[] AllTypes
		{
			get
			{
				if (allTypes == null)
				{
					var types = new List<Type>(512);
					var assemblies = AppDomain.CurrentDomain.GetAssemblies();

					foreach (var assembly in assemblies)
						types.AddRange(assembly.GetTypes());

					allTypes = types.ToArray();
				}

				return allTypes;
			}
		}

		public static Type[] GetSubclasses(Type baseType)
		{
			Type[] types;

			if (!typeToSubclassTypes.TryGetValue(baseType, out types))
			{
				var typeList = new List<Type>();

				foreach (var type in AllTypes)
				{
					if (type.IsSubclassOf(baseType))
						typeList.Add(type);
				}

				types = typeList.ToArray();
				typeToSubclassTypes[baseType] = types;
			}

			return types;
		}

		public static Type[] GetAssignableTypes(Type baseType, bool includeSelf = true)
		{
			Type[] types;

			if (!typeToAssignableTypes.TryGetValue(baseType, out types))
			{
				var typeList = new List<Type>();

				foreach (var type in AllTypes)
				{
					if ((type != baseType || includeSelf) && baseType.IsAssignableFrom(type))
						typeList.Add(type);
				}

				types = typeList.ToArray();
				typeToAssignableTypes[baseType] = types;
			}

			return types;
		}

		public static Type[] GetDefinedTypes(Type attributeType)
		{
			Type[] types;

			if (!typeToDefinedTypes.TryGetValue(attributeType, out types))
			{
				var typeList = new List<Type>();

				foreach (var type in AllTypes)
				{
					if (type.IsDefined(attributeType, true))
						typeList.Add(type);
				}

				types = typeList.ToArray();
				typeToDefinedTypes[attributeType] = types;
			}

			return types;
		}

		public static FieldInfo[] GetAllFields(Type type)
		{
			FieldInfo[] fields;

			if (!typeToFields.TryGetValue(type, out fields))
			{
				fields = type.GetFields(ReflectionExtensions.AllFlags);
				typeToFields[type] = fields;
			}

			return fields;
		}

		public static object GetDefaultValue(Type type)
		{
			object defaultValue;

			if (!typeToDefaultValue.TryGetValue(type, out defaultValue))
			{
				if (type.IsValueType)
					defaultValue = Activator.CreateInstance(type);
				else
					defaultValue = null;

				typeToDefaultValue[type] = defaultValue;
			}

			return defaultValue;

		}

		public static Type GetType(string typeName)
		{
			Type type;

			if (!typeNameToType.TryGetValue(typeName, out type))
			{
				type = Type.GetType(typeName);
				typeNameToType[typeName] = type;
			}

			return type;
		}

		public static IEqualityComparer<T> GetEqualityComparer<T>()
		{
			return EqualityComparerHolder<T>.Comparer;
		}

		static class EqualityComparerHolder<T>
		{
			public static IEqualityComparer<T> Comparer = CreateComparer();

			static IEqualityComparer<T> CreateComparer()
			{
				var comparerType = Array.Find(GetAssignableTypes(typeof(IEqualityComparer<T>), false), t => !t.IsInterface && !t.IsAbstract && t.HasEmptyConstructor());

				if (comparerType == null)
					return EqualityComparer<T>.Default;
				else
					return (IEqualityComparer<T>)Activator.CreateInstance(comparerType);
			}
		}
	}
}
