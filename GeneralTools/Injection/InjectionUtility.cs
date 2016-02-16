using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;

namespace Pseudo.Internal.Injection
{
	public static class InjectionUtility
	{
		public static readonly object[] Empty = new object[0];

		static readonly Dictionary<Type, IInjectableConstructor[]> typeToInjectableConstructors = new Dictionary<Type, IInjectableConstructor[]>();
		static readonly Dictionary<Type, IInjectableMember[]> typeToInjectableMembers = new Dictionary<Type, IInjectableMember[]>();

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
			var constructors = type.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

			if (Array.TrueForAll(constructors, c => !c.IsDefined(typeof(InjectAttribute), true)))
				return constructors
					.Where(c => !c.IsStatic)
					.Select(c => (IInjectableConstructor)new InjectableConstructor(c, CreateInjectableParameters(c)))
					.ToArray();
			else
				return constructors
					.Where(c => !c.IsStatic)
					.Select(c => (IInjectableConstructor)new InjectableConstructor(c, CreateInjectableParameters(c)))
					.ToArray();
		}

		static IInjectableMember[] CreateInjectableMembers(Type type)
		{
			var injectableMembers = new List<IInjectableMember>();

			injectableMembers.AddRange(type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
				.Where(f => f.IsDefined(typeof(InjectAttribute), true))
				.Select(f => (IInjectableMember)new InjectableField(f)));

			injectableMembers.AddRange(type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
				.Where(p => p.CanWrite && p.IsDefined(typeof(InjectAttribute), true))
				.Select(p => (IInjectableMember)new InjectableProperty(p)));

			injectableMembers.AddRange(type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
				.Where(m => m.IsDefined(typeof(InjectAttribute), true))
				.Select(m => (IInjectableMember)new InjectableMethod(m, CreateInjectableParameters(m))));

			return injectableMembers.ToArray();
		}

		static IInjectableParameter[] CreateInjectableParameters(ConstructorInfo constructor)
		{
			return constructor.GetParameters().Select(p => (IInjectableParameter)new InjectableConstructorParameter(constructor, p)).ToArray();
		}

		static IInjectableParameter[] CreateInjectableParameters(MethodInfo method)
		{
			return method.GetParameters().Select(m => (IInjectableParameter)new InjectableMethodParameter(method, m)).ToArray();
		}
	}
}
