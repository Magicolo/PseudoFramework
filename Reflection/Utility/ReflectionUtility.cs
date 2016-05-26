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
		public const BindingFlags InstanceFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

		public static readonly object[] EmptyArguments = new object[0];

		public static ITypeWrapper CreateTypeWrapper(Type type, BindingFlags flags = InstanceFlags, Func<MemberInfo, bool> filter = null)
		{
			filter = filter ?? delegate { return true; };

			return new TypeWrapper
			{
				Type = type,
				Constructors = CreateConstructorWrappers(type, flags, c => filter(c)).ToArray(),
				Fields = CreateFieldWrappers(type, flags, f => filter(f)).ToArray(),
				Properties = CreatePropertyWrappers(type, flags, p => filter(p)).ToArray(),
				Methods = CreateMethodWrappers(type, flags, m => filter(m)).ToArray(),
			};
		}

		public static IEnumerable<IConstructorWrapper> CreateConstructorWrappers(Type type, BindingFlags flags = InstanceFlags, Func<ConstructorInfo, bool> filter = null)
		{
			filter = filter ?? delegate { return true; };

			return type.GetConstructors(flags)
				.Where(filter)
				.Select(c => CreateConstructorWrapper(c));
		}

		public static IConstructorWrapper CreateConstructorWrapper(ConstructorInfo constructor)
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

		public static IConstructorWrapper CreateConstructorWrapper(Type type)
		{
			if (type.IsValueType)
				return new EmptyConstructorWrapper(type);
			else
			{
				var constructor = type.GetConstructor(Type.EmptyTypes);

				return constructor == null ? null : CreateConstructorWrapper(constructor);
			}
		}

		public static IEnumerable<IFieldWrapper> CreateFieldWrappers(Type type, BindingFlags flags = InstanceFlags, Func<FieldInfo, bool> filter = null)
		{
			filter = filter ?? delegate { return true; };

			return type.GetFields(flags)
				.Where(filter)
				.Select(f => CreateFieldWrapper(f));
		}

		public static IFieldWrapper CreateFieldWrapper(FieldInfo field)
		{
			if (ApplicationUtility.IsAOT)
				return new FieldWrapper(field);
			else
			{
				var wrapperType = typeof(FieldWrapper<,>).MakeGenericType(field.DeclaringType, field.FieldType);

				return (IFieldWrapper)Activator.CreateInstance(wrapperType, field);
			}
		}

		public static IEnumerable<IPropertyWrapper> CreatePropertyWrappers(Type type, BindingFlags flags = InstanceFlags, Func<PropertyInfo, bool> filter = null)
		{
			filter = filter ?? delegate { return true; };

			return type.GetProperties(flags)
				.Where(filter)
				.Select(p => CreatePropertyWrapper(p));
		}

		public static IPropertyWrapper CreatePropertyWrapper(PropertyInfo property)
		{
			if (property.IsAutoProperty())
				return CreateFieldWrapper(property.GetBackingField());
			else if (ApplicationUtility.IsAOT)
				return new PropertyWrapper(property);
			else
			{
				var wrapperType = typeof(PropertyWrapper<,>).MakeGenericType(property.DeclaringType, property.PropertyType);

				return (IPropertyWrapper)Activator.CreateInstance(wrapperType, property);
			}
		}

		public static IEnumerable<IMethodWrapper> CreateMethodWrappers(Type type, BindingFlags flags = InstanceFlags, Func<MethodInfo, bool> filter = null)
		{
			filter = filter ?? delegate { return true; };

			return type.GetMethods(flags)
				.Where(filter)
				.Select(m => CreateMethodWrapper(m));
		}

		public static IMethodWrapper CreateMethodWrapper(MethodInfo method)
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
	}
}
