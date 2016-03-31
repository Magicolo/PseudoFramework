using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;
using Pseudo.Internal;

namespace Pseudo
{
	public static class TypeUtility
	{
		static readonly Dictionary<Type, object> typeToDefaultValue = new Dictionary<Type, object>();
		static readonly Dictionary<string, Type> typeNameToType = new Dictionary<string, Type>();

		static Type[] allTypes;
		public static Type[] AllTypes
		{
			get
			{
				if (allTypes == null)
					allTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()).ToArray();

				return allTypes;
			}
		}

		public static IEnumerable<Type> GetBaseTypes(Type type, bool includeSelf = true, bool includeInterfaces = true)
		{
			var types = new HashSet<Type>();
			var baseType = includeSelf ? type : type.BaseType;

			if (includeInterfaces)
			{
				var interfaces = type.GetInterfaces();

				for (int i = 0; i < interfaces.Length; i++)
					types.Add(interfaces[i]);
			}

			while (baseType != null)
			{
				types.Add(baseType);

				if (includeInterfaces)
				{
					var interfaces = baseType.GetInterfaces();

					for (int i = 0; i < interfaces.Length; i++)
						types.Add(interfaces[i]);
				}

				baseType = baseType.BaseType;
			}

			return types;
		}

		public static IEnumerable<Type> GetAssignableTypes(Type baseType, bool includeSelf = true)
		{
			return AllTypes.Where(t => (includeSelf || t != baseType) && baseType.IsAssignableFrom(t));
		}

		public static IEnumerable<Type> GetDefinedTypes(Type attributeType)
		{
			return AllTypes.Where(t => t.IsDefined(attributeType, true));
		}

		public static Type FindType(Predicate<Type> match)
		{
			return Array.Find(AllTypes, match);
		}

		public static IEnumerable<Type> FindTypes(Predicate<Type> match)
		{
			return AllTypes.Where(t => match(t));
		}

		public static object GetDefaultValue(Type type)
		{
			if (type.IsClass)
				return null;

			object defaultValue;

			if (!typeToDefaultValue.TryGetValue(type, out defaultValue))
			{
				defaultValue = Activator.CreateInstance(type);
				typeToDefaultValue[type] = defaultValue;
			}

			return defaultValue;

		}

		public static Type GetType(string typeName)
		{
			Type type;

			if (!typeNameToType.TryGetValue(typeName, out type))
			{
				type = Type.GetType(typeName);
				typeNameToType[typeName] = type;
			}

			return type;
		}
	}
}
