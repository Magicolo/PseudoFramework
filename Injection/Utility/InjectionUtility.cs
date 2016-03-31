using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;
using Pseudo.Internal;

namespace Pseudo.Injection.Internal
{
	public static class InjectionUtility
	{
		public static readonly object[] Empty = new object[0];

		static readonly Dictionary<Type, IInjectableConstructor[]> typeToInjectableConstructors = new Dictionary<Type, IInjectableConstructor[]>();
		static readonly Dictionary<Type, IInjectableMember[]> typeToInjectableMembers = new Dictionary<Type, IInjectableMember[]>();
		static readonly Func<ConstructorInfo, bool> constructorFilter = c => c.IsDefined(typeof(InjectAttribute), true);
		static readonly Func<FieldInfo, bool> fieldFilter = f => !f.IsSpecialName && f.IsDefined(typeof(InjectAttribute), true);
		static readonly Func<PropertyInfo, bool> propertyFilter = p => !p.IsSpecialName && p.CanWrite && p.IsDefined(typeof(InjectAttribute), true);
		static readonly Func<MethodInfo, bool> methodFilter = m => !m.IsSpecialName && !m.IsConstructor && !m.IsGenericMethod && m.IsDefined(typeof(InjectAttribute), true);

		public static IInjectableConstructor[] GetInjectableConstructors(Type type)
		{
			IInjectableConstructor[] injectableConstructors;

			if (!typeToInjectableConstructors.TryGetValue(type, out injectableConstructors))
			{
				injectableConstructors = CreateInjectableConstructors(type);
				typeToInjectableConstructors[type] = injectableConstructors;
			}

			return injectableConstructors;
		}

		public static IInjectableMember[] GetInjectableMembers(Type type)
		{
			IInjectableMember[] injectableMembers;

			if (!typeToInjectableMembers.TryGetValue(type, out injectableMembers))
			{
				injectableMembers = CreateInjectableMembers(type);
				typeToInjectableMembers[type] = injectableMembers;
			}

			return injectableMembers;
		}

		static IInjectableConstructor[] CreateInjectableConstructors(Type type)
		{
			var injectableConstructors = new List<IInjectableConstructor>();
			var constructors = type.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

			// At least one constructor has an [Inject].
			if (constructors.Any(constructorFilter))
				injectableConstructors.AddRange(constructors
					.Where(constructorFilter)
					.Select(c => CreateInjectableConstructor(c)));
			else
			{
				injectableConstructors.AddRange(constructors
					.Select(c => CreateInjectableConstructor(c)));

				// Should be added last to be evaluated after all other constructors.
				if (type.IsValueType)
					injectableConstructors.Add(CreateInjectableConstructor(type));
			}

			return injectableConstructors.ToArray();
		}

		static IInjectableMember[] CreateInjectableMembers(Type type)
		{
			var injectableMembers = new List<IInjectableMember>();
			var baseTypes = TypeUtility.GetBaseTypes(type, false, false);

			injectableMembers.AddRange(type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
				.Where(fieldFilter)
				.Concat(baseTypes // Need to recover the private members from base types.
				.SelectMany(t => t.GetFields(BindingFlags.Instance | BindingFlags.NonPublic))
				.Where(f => fieldFilter(f) && f.IsPrivate))
				.Select(f => CreateInjectableField(f)));

			injectableMembers.AddRange(type.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
				.Where(propertyFilter)
				.Concat(baseTypes // Need to recover the private members from base types.
				.SelectMany(t => t.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic))
				.Where(p => propertyFilter(p) && p.IsPrivate()))
				.Select(p => CreateInjectableProperty(p)));

			injectableMembers.AddRange(type.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
				.Where(methodFilter)
				.Concat(baseTypes // Need to recover the private members from base types.
				.SelectMany(t => t.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic))
				.Where(m => methodFilter(m) && m.IsPrivate))
				.Select(m => CreateInjectableMethod(m)));

			return injectableMembers.ToArray();
		}

		static IInjectableConstructor CreateInjectableConstructor(ConstructorInfo constructor)
		{
			return new InjectableConstructor(constructor, CreateInjectableParameters(constructor.GetParameters()));
		}

		static IInjectableConstructor CreateInjectableConstructor(Type valueType)
		{
			return new InjectableEmptyStructConstructor(valueType);
		}

		static IInjectableMember CreateInjectableField(FieldInfo field)
		{
			return new InjectableField(field);
		}

		static IInjectableMember CreateInjectableProperty(PropertyInfo property)
		{
			if (property.IsAutoProperty())
				return new InjectableAutoProperty(property);
			else
				return new InjectableProperty(property);
		}

		static IInjectableMember CreateInjectableMethod(MethodInfo method)
		{
			if (method.ReturnType == typeof(void) && method.GetParameters().Length == 0)
				return new InjectableEmptyMethod(method);
			else
				return new InjectableMethod(method, CreateInjectableParameters(method.GetParameters()));
		}

		static IInjectableParameter[] CreateInjectableParameters(IEnumerable<ParameterInfo> parameters)
		{
			return parameters.Select(p => (IInjectableParameter)new InjectableParameter(p)).ToArray();
		}
	}
}
