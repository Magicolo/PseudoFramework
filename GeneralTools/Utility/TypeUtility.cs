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

		public static Type[] GetSubclasses(Type baseType)
		{
			return AllTypes.Where(t => t.IsSubclassOf(baseType)).ToArray();
		}

		public static Type[] GetAssignableTypes(Type baseType, bool includeSelf = true)
		{
			return AllTypes.Where(t => (includeSelf || t != baseType) && baseType.IsAssignableFrom(t)).ToArray();
		}

		public static Type[] GetDefinedTypes(Type attributeType)
		{
			return AllTypes.Where(t => t.IsDefined(attributeType, true)).ToArray();
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
