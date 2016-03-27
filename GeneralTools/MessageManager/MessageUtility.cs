using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;

namespace Pseudo.Internal.Communication
{
	public static class MessageUtility
	{
		static readonly Dictionary<Type, MethodInfo[]> typeToMessageMethods = new Dictionary<Type, MethodInfo[]>();
		static readonly Dictionary<MethodInfo, object[]> methodToAttributes = new Dictionary<MethodInfo, object[]>();
		static readonly Dictionary<MethodInfo, Type> methodToActionType = new Dictionary<MethodInfo, Type>();

		public static Delegate CreateMethod<TId>(object target, TId identifier)
		{
			var method = GetValidMethod(target.GetType(), identifier);

			if (method == null)
				return null;

			var actionType = GetActionType(method);

			if (actionType == null)
				return null;

			return Delegate.CreateDelegate(actionType, target, method);
		}

		static MethodInfo GetValidMethod<TId>(Type type, TId identifier)
		{
			var methods = GetMessageMethods(type);

			for (int i = 0; i < methods.Length; i++)
			{
				var method = methods[i];
				var attributes = GetAttributes(method);

				for (int j = 0; j < attributes.Length; j++)
				{
					var attribute = (MessageAttribute)attributes[j];

					if (attribute.Identifier is TId && PEqualityComparer<TId>.Default.Equals(identifier, (TId)attribute.Identifier))
						return method;
				}
			}

			return null;
		}

		static object[] GetAttributes(MethodInfo method)
		{
			object[] attributes;

			if (!methodToAttributes.TryGetValue(method, out attributes))
			{
				attributes = method.GetCustomAttributes(typeof(MessageAttribute), true);
				methodToAttributes[method] = attributes;
			}

			return attributes;
		}

		static MethodInfo[] GetMessageMethods(Type type)
		{
			MethodInfo[] methods;

			if (!typeToMessageMethods.TryGetValue(type, out methods))
			{
				methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
					.Where(m => !m.IsGenericMethod && m.IsDefined(typeof(MessageAttribute), true))
					.ToArray();
				typeToMessageMethods[type] = methods;
			}

			return methods;
		}

		static Type GetActionType(MethodInfo method)
		{
			Type actionType;

			if (!methodToActionType.TryGetValue(method, out actionType))
			{
				var parameterTypes = method.GetParameters().Convert(p => p.ParameterType);

				switch (parameterTypes.Length)
				{
					case 0:
						actionType = typeof(Action);
						break;
					case 1:
						actionType = typeof(Action<>).MakeGenericType(parameterTypes);
						break;
				}

				methodToActionType[method] = actionType;
			}

			return actionType;
		}
	}
}
