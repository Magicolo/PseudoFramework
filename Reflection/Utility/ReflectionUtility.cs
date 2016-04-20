using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;
using Pseudo.Reflection.Internal;

namespace Pseudo.Reflection
{
	public static class ReflectionUtility
	{
		public const BindingFlags AllFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.FlattenHierarchy;
		public const BindingFlags PublicFlags = BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.FlattenHierarchy;
		public const BindingFlags NonPublicFlags = BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.FlattenHierarchy;
		public const BindingFlags StaticFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.FlattenHierarchy;
		public const BindingFlags InstanceFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;

		public static readonly object[] EmptyArguments = new object[0];

		public static IMemberWrapper CreateWrapper(FieldInfo field)
		{
			if (ApplicationUtility.IsAOT)
				return new FieldWrapper(field);
			else
			{
				var wrapperType = typeof(FieldWrapper<,>).MakeGenericType(field.DeclaringType, field.FieldType);

				return (IMemberWrapper)Activator.CreateInstance(wrapperType, field);
			}
		}

		public static IMemberWrapper CreateWrapper(PropertyInfo property)
		{
			if (property.IsAutoProperty())
				return CreateWrapper(property.GetBackingField());
			else if (ApplicationUtility.IsAOT)
				return new PropertyWrapper(property);
			else
			{
				var wrapperType = typeof(PropertyWrapper<,>).MakeGenericType(property.DeclaringType, property.PropertyType);

				return (IMemberWrapper)Activator.CreateInstance(wrapperType, property);
			}
		}

		public static IMethodWrapper CreateWrapper(MethodInfo method)
		{
			var parameters = method.GetParameters();
			Type wrapperType;
			Type[] genericArguments;

			if (ApplicationUtility.IsAOT)
				return new MethodWrapper(method);
			else if (method.ReturnType == typeof(void))
			{
				genericArguments = new[] { method.DeclaringType }
					.Concat(parameters.Select(p => p.ParameterType))
					.ToArray();

				switch (parameters.Length)
				{
					default:
						return new MethodWrapper(method);
					case 0:
						wrapperType = typeof(MethodWrapper<>);
						break;
					case 1:
						wrapperType = typeof(MethodWrapperIn<,>);
						break;
					case 2:
						wrapperType = typeof(MethodWrapperIn<,,>);
						break;
					case 3:
						wrapperType = typeof(MethodWrapperIn<,,,>);
						break;
				}
			}
			else
			{
				genericArguments = new[] { method.DeclaringType }
					.Concat(parameters.Select(p => p.ParameterType))
					.Concat(new[] { method.ReturnType })
					.ToArray();

				switch (parameters.Length)
				{
					default:
						return new MethodWrapper(method);
					case 0:
						wrapperType = typeof(MethodWrapperOut<,>);
						break;
					case 1:
						wrapperType = typeof(MethodWrapperInOut<,,>);
						break;
					case 2:
						wrapperType = typeof(MethodWrapperInOut<,,,>);
						break;
					case 3:
						wrapperType = typeof(MethodWrapperInOut<,,,,>);
						break;
				}
			}

			return (IMethodWrapper)Activator.CreateInstance(wrapperType.MakeGenericType(genericArguments), method);
		}

		public static IConstructorWrapper CreateWrapper(ConstructorInfo constructor)
		{
			var parameters = constructor.GetParameters();

			if (ApplicationUtility.IsAOT)
				return new ConstructorWrapper(constructor);
			else
			{
				Type wrapperType;
				var genericArguments = new[] { constructor.DeclaringType }
					.Concat(parameters
					.Select(p => p.ParameterType))
					.ToArray();

				switch (parameters.Length)
				{
					default:
						return new ConstructorWrapper(constructor);
					case 0:
						wrapperType = typeof(ConstructorWrapper<>);
						break;
					case 1:
						wrapperType = typeof(ConstructorWrapper<,>);
						break;
					case 2:
						wrapperType = typeof(ConstructorWrapper<,,>);
						break;
					case 3:
						wrapperType = typeof(ConstructorWrapper<,,,>);
						break;
				}

				return (IConstructorWrapper)Activator.CreateInstance(wrapperType.MakeGenericType(genericArguments), constructor);
			}
		}

		public static IConstructorWrapper CreateWrapper(Type type)
		{
			if (type.IsValueType)
				return new EmptyConstructorWrapper(type);
			else
			{
				var constructor = type.GetConstructor(Type.EmptyTypes);

				return constructor == null ? null : CreateWrapper(constructor);
			}
		}
	}
}
