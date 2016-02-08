using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using System.Reflection;

namespace Pseudo.Internal.EntityOld
{
	public class MessagerGroup
	{
		public const int maxParameters = 1;
		static readonly Dictionary<Type, MethodInfo[]> typeMethodInfos = new Dictionary<Type, MethodInfo[]>();
		static readonly Dictionary<MethodInfo, Type[]> methodInfoParameterTypes = new Dictionary<MethodInfo, Type[]>();

		readonly string method;
		readonly Dictionary<Type, IMessager> messagers = new Dictionary<Type, IMessager>();

		static MethodInfo[] GetMethodInfos(Type type)
		{
			MethodInfo[] methodInfos;

			if (!typeMethodInfos.TryGetValue(type, out methodInfos))
			{
				methodInfos = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
				typeMethodInfos[type] = methodInfos;
			}

			return methodInfos;
		}

		static Type[] GetParameterTypes(MethodInfo methodInfo)
		{
			Type[] parameterTypes;

			if (!methodInfoParameterTypes.TryGetValue(methodInfo, out parameterTypes))
			{
				parameterTypes = methodInfo.GetParameters().Convert(parameter => parameter.ParameterType);
				methodInfoParameterTypes[methodInfo] = parameterTypes;
			}

			return parameterTypes;
		}

		public MessagerGroup(string method)
		{
			this.method = method;
		}

		public IMessager GetMessager(Type type)
		{
			IMessager messager;

			if (!messagers.TryGetValue(type, out messager))
			{
				messager = CreateMessager(type);
				messagers[type] = messager;
			}

			return messager;
		}

		IMessager CreateMessager(Type type)
		{
			IMessager messager = null;
			var methodInfos = GetMethodInfos(type);

			for (int i = 0; i < methodInfos.Length; i++)
			{
				var methodInfo = methodInfos[i];

				if (methodInfo.Name == method)
				{
					var parameterTypes = GetParameterTypes(methodInfo);

					if (parameterTypes.Length > maxParameters)
						continue;

					messager = CreateMessager(methodInfo, parameterTypes);
					break;
				}
			}

			return messager;
		}

		IMessager CreateMessager(MethodInfo methodInfo, Type[] parameterTypes)
		{
			Type actionType;
			Type messagerType;
			Type[] genericArguments = new Type[parameterTypes.Length + 1];
			genericArguments[0] = methodInfo.DeclaringType;
			parameterTypes.CopyTo(genericArguments, 1);

			if (parameterTypes.Length == 0)
			{
				actionType = typeof(Action<>).MakeGenericType(genericArguments);
				messagerType = typeof(Messager<>).MakeGenericType(genericArguments);

			}
			else
			{
				actionType = typeof(Action<,>).MakeGenericType(genericArguments);
				messagerType = typeof(Messager<,>).MakeGenericType(genericArguments);
			}

			Delegate action = Delegate.CreateDelegate(actionType, methodInfo);
			return (IMessager)Activator.CreateInstance(messagerType, action);
		}
	}
}