using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using System.Collections;
using Pseudo.Internal;

namespace Pseudo
{
	public static class TypeExtensions
	{
		public static Type[] GetSubclasses(this Type baseType)
		{
			return TypeUtility.GetAssignableTypes(baseType);
		}

		public static Type[] GetAssignableTypes(this Type baseType, bool includeSelf = true)
		{
			return TypeUtility.GetAssignableTypes(baseType, includeSelf);
		}

		public static Type[] GetDefinedTypes(this Type attributeType)
		{
			return TypeUtility.GetDefinedTypes(attributeType);
		}

		public static FieldInfo[] GetAllFields(this Type type)
		{
			return TypeUtility.GetAllFields(type);
		}

		public static object CreateDefaultInstance(this Type type)
		{
			object instance = null;

			if (type == typeof(string))
				instance = string.Empty;
			else
				instance = Activator.CreateInstance(type, type.GetDefaultConstructorParameters());

			return instance;
		}

		public static object[] GetDefaultConstructorParameters(this Type type)
		{
			List<object> parameters = new List<object>();

			if (!type.HasEmptyConstructor() && type.HasConstructor())
			{
				ParameterInfo[] parameterInfos = type.GetConstructors()[0].GetParameters();

				for (int i = 0; i < parameterInfos.Length; i++)
				{
					ParameterInfo info = parameterInfos[i];
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
			return interfaceType.IsAssignableFrom(type) || Array.Exists(type.GetInterfaces(), t => t.IsGenericType && t.GetGenericTypeDefinition() == interfaceType);
		}

		public static bool Is(this Type type, Type otherType, params Type[] genericArguments)
		{
			if (genericArguments.Length > 0)
				return otherType.MakeGenericType(genericArguments).IsAssignableFrom(type);
			else
				return otherType.IsAssignableFrom(type);
		}

		public static bool IsNumeric(this Type type)
		{
			return
				type == typeof(sbyte) ||
				type == typeof(byte) ||
				type == typeof(short) ||
				type == typeof(ushort) ||
				type == typeof(int) ||
				type == typeof(uint) ||
				type == typeof(long) ||
				type == typeof(ulong) ||
				type == typeof(float) ||
				type == typeof(double) ||
				type == typeof(decimal);
		}

		public static bool IsVector(this Type type)
		{
			return type == typeof(Vector2) || type == typeof(Vector3) || type == typeof(Vector4);
		}

		public static string GetName(this Type type)
		{
			return type.Name.Split('.').Last().GetRange('`');
		}
	}
}
