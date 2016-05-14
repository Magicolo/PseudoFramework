using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;
using Pseudo.Reflection;

namespace Pseudo.PoolingNOOOO.Internal
{
	public class TypeAnalyzer : ITypeAnalyzer
	{
		static readonly Func<FieldInfo, bool> fieldFilter = f => !f.IsSpecialName && !f.IsDefined(typeof(DoNotInitializeAttribute), true);
		static readonly Func<PropertyInfo, bool> propertyFilter = p => !p.IsSpecialName && p.CanRead && p.CanWrite && !p.IsDefined(typeof(DoNotInitializeAttribute), true) && (p.IsDefined(typeof(InitializeAttribute), true) || p.IsDefined(typeof(InitializeContentAttribute), true));

		readonly Dictionary<Type, ITypeInfo> typeToInjectionInfo = new Dictionary<Type, ITypeInfo>();

		public ITypeInfo Analyze(Type type)
		{
			ITypeInfo info;

			if (!typeToInjectionInfo.TryGetValue(type, out info))
			{
				info = CreateTypeInfo(type);
				typeToInjectionInfo[type] = info;
			}

			return info;
		}

		ITypeInfo CreateTypeInfo(Type type)
		{
			var baseTypes = TypeUtility.GetBaseTypes(type, false, false).ToArray();

			return new TypeInfo
			{
				Type = type,
				BaseTypes = baseTypes,
				Fields = CreateInitializableFields(type, baseTypes)
			};
		}

		IInitializableField[] CreateInitializableFields(Type type, Type[] baseTypes)
		{
			return type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
				.Concat(baseTypes // Need to recover the private members from base types.
					.SelectMany(t => t.GetFields(BindingFlags.Instance | BindingFlags.NonPublic))
					.Where(f => f.IsPrivate))
				.Where(fieldFilter)
				.Select(f => CreateInitializableField(f))
				.ToArray();
		}

		IInitializableField CreateInitializableField(FieldInfo field)
		{
			return new InitializableField(field);
		}

		IInitializableProperty[] CreateInitializableProperties(Type type, Type[] baseTypes)
		{
			return type.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
				.Concat(baseTypes // Need to recover the private members from base types.
					.SelectMany(t => t.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic))
					.Where(p => p.IsPrivate()))
				.Where(propertyFilter)
				.Select(p => CreateInitializableProperty(p))
				.ToArray();
		}

		IInitializableProperty CreateInitializableProperty(PropertyInfo property)
		{
			return new InitializableProperty(property);
		}
	}
}
