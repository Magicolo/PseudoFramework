using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using System.Collections;

namespace Pseudo
{
	public static class TypeExtensions
	{
		static Dictionary<Type, Type[]> AssignableTypesDict = new Dictionary<Type, Type[]>();
		static Dictionary<Type, Type[]> SubclassTypes = new Dictionary<Type, Type[]>();

		static Type[] _allTypes;
		public static Type[] AllTypes
		{
			get
			{
				if (_allTypes == null)
				{
					List<Type> types = new List<Type>();
					Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

					for (int i = 0; i < assemblies.Length; i++)
					{
						Assembly assembly = assemblies[i];

						types.AddRange(assembly.GetTypes());
					}

					_allTypes = types.ToArray();
				}

				return _allTypes;
			}
		}

		public static Type[] GetSubclassTypes(this Type type)
		{
			if (!SubclassTypes.ContainsKey(type))
			{
				List<Type> derivedTypes = new List<Type>();

				for (int i = 0; i < AllTypes.Length; i++)
				{
					Type derivedType = AllTypes[i];

					if (derivedType.IsSubclassOf(type))
						derivedTypes.Add(derivedType);
				}

				SubclassTypes[type] = derivedTypes.ToArray();
			}

			return SubclassTypes[type];
		}

		public static Type[] GetAssignableTypes(this Type type)
		{
			if (!AssignableTypesDict.ContainsKey(type))
			{
				List<Type> derivedTypes = new List<Type>();

				for (int i = 0; i < AllTypes.Length; i++)
				{
					Type derivedType = AllTypes[i];

					if (type.IsAssignableFrom(derivedType))
						derivedTypes.Add(derivedType);
				}

				AssignableTypesDict[type] = derivedTypes.ToArray();
			}

			return AssignableTypesDict[type];
		}

		public static object CreateDefaultInstance(this Type type)
		{
			object instance = null;

			if (type == typeof(string))
			{
				instance = string.Empty;
			}
			else
			{
				instance = Activator.CreateInstance(type, type.GetDefaultConstructorParameters());
			}

			return instance;
		}

		public static object[] GetDefaultConstructorParameters(this Type type)
		{
			List<object> parameters = new List<object>();

			if (!type.HasEmptyConstructor() && type.HasConstructor())
			{
				ParameterInfo[] parameterInfos = type.GetConstructors()[0].GetParameters();

				foreach (ParameterInfo info in parameterInfos)
				{
					parameters.Add(info.ParameterType.CreateDefaultInstance());
				}
			}

			return parameters.ToArray();
		}

		public static bool HasConstructor(this Type type)
		{
			return type.GetConstructors().Length > 0;
		}

		public static bool HasEmptyConstructor(this Type type)
		{
			return type.GetConstructor(Type.EmptyTypes) != null;
		}

		public static bool HasDefaultConstructor(this Type type)
		{
			return type.IsValueType || type.HasEmptyConstructor();
		}

		public static bool HasInterface(this Type type, Type interfaceType)
		{
			return interfaceType.IsAssignableFrom(type) || Array.Exists(type.GetInterfaces(), iType => iType.IsGenericType && iType.GetGenericTypeDefinition() == interfaceType);
		}

		public static bool IsNumerical(this Type type)
		{
			return type == typeof(int) || type == typeof(float) || type == typeof(double);
		}

		public static bool IsVector(this Type type)
		{
			return type == typeof(Vector2) || type == typeof(Vector3) || type == typeof(Vector4);
		}

		public static string GetName(this Type type)
		{
			return type.Name.Split('.').Last().GetRange('`');
		}

		public static string[] GetFieldsPropertiesNames(this Type type, BindingFlags flags, params Type[] filter)
		{
			List<string> names = new List<string>();

			foreach (FieldInfo field in type.GetFields(flags))
			{
				if (filter == null || filter.Length == 0 || filter.Any(t => t.IsAssignableFrom(field.FieldType)))
				{
					names.Add(field.Name);
				}
			}

			foreach (PropertyInfo property in type.GetProperties(flags))
			{
				if (filter == null || filter.Length == 0 || filter.Any(t => t.IsAssignableFrom(property.PropertyType)))
				{
					names.Add(property.Name);
				}
			}
			return names.ToArray();
		}

		public static string[] GetFieldsPropertiesNames(this Type type, params Type[] filter)
		{
			return GetFieldsPropertiesNames(type, ObjectExtensions.AllFlags, filter);
		}

#if UNITY_EDITOR
		[UnityEditor.Callbacks.DidReloadScripts]
		static void OnScriptReload()
		{
			AssignableTypesDict = new Dictionary<Type, Type[]>();
			SubclassTypes = new Dictionary<Type, Type[]>();
		}
#endif
	}
}
