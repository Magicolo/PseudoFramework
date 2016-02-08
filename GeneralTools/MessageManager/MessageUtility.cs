using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;

namespace Pseudo.Internal
{
	public static class MessageUtility
	{
		static readonly Dictionary<Type, MethodInfo[]> typeToMethods = new Dictionary<Type, MethodInfo[]>();
		static readonly Dictionary<MethodInfo, Type[]> methodToParameterTypes = new Dictionary<MethodInfo, Type[]>();
		static readonly Dictionary<MethodInfo, object[]> methodToAttributes = new Dictionary<MethodInfo, object[]>();

		public static MethodInfo GetValidMethod<TId>(Type type, TId identifier)
		{
			var methods = GetMethods(type);

			for (int i = 0; i < methods.Length; i++)
			{
				var method = methods[i];
				var attributes = GetAttributes(method);

				for (int j = 0; j < attributes.Length; j++)
				{
					var attribute = (MessageAttribute)attributes[j];
					var comparer = TypeUtility.GetEqualityComparer<TId>();

					if (attribute.Identifier != null &&
						typeof(TId).IsAssignableFrom(attribute.Identifier.GetType()) &&
						comparer.Equals(identifier, (TId)attribute.Identifier))
						return method;
				}
			}

			return null;
		}

		public static Type[] GetParameterTypes(MethodInfo method)
		{
			Type[] parameterTypes;

			if (!methodToParameterTypes.TryGetValue(method, out parameterTypes))
			{
				parameterTypes = new[] { method.DeclaringType }.Concat(method.GetParameters().Convert(p => p.ParameterType)).ToArray();
				methodToParameterTypes[method] = parameterTypes;
			}

			return parameterTypes;
		}

		public static object[] GetAttributes(MethodInfo method)
		{
			object[] attributes;

			if (!methodToAttributes.TryGetValue(method, out attributes))
			{
				attributes = method.GetCustomAttributes(typeof(MessageAttribute), true);
				methodToAttributes[method] = attributes;
			}

			return attributes;
		}

		static MethodInfo[] GetMethods(Type type)
		{
			MethodInfo[] methods;

			if (!typeToMethods.TryGetValue(type, out methods))
			{
				methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				typeToMethods[type] = methods;
			}

			return methods;
		}
	}
}
