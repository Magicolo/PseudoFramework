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
		// Cache for ITypeInfo because of expensive Type analysis.
		static readonly Dictionary<Type, ITypeInfo> typeToInjectionInfo = new Dictionary<Type, ITypeInfo>();

		public static ITypeInfo GetInjectionInfo(Type type)
		{
			ITypeInfo info;

			if (!typeToInjectionInfo.TryGetValue(type, out info))
			{
				info = new TypeInfo(type);
				typeToInjectionInfo[type] = info;
			}

			return info;
		}

		public static Predicate<InjectionContext> GetCondition(ConditionSource conditionSource, ConditionComparer conditionComparer, object conditionTarget)
		{
			return c =>
			{
				switch (conditionSource)
				{
					default:
					case ConditionSource.ContextType:
						return ConditionComparer(c.ContextType, (InjectionContext.ContextTypes)conditionTarget, conditionComparer);
					case ConditionSource.ContractType:
						return ConditionComparer(c.ContractType, (Type)conditionTarget, conditionComparer);
					case ConditionSource.DeclaringType:
						return ConditionComparer(c.DeclaringType, (Type)conditionTarget, conditionComparer);
					case ConditionSource.Identifier:
						return ConditionComparer(c.Identifier, (string)conditionTarget, conditionComparer);
					case ConditionSource.Optional:
						return ConditionComparer(c.Optional, (bool)conditionTarget, conditionComparer);
				}
			};
		}

		static bool ConditionComparer<T>(T source, T target, ConditionComparer conditionComparer)
		{
			switch (conditionComparer)
			{
				default:
				case Injection.ConditionComparer.Equals:
					return PEqualityComparer<T>.Default.Equals(source, target);
				case Injection.ConditionComparer.NotEquals:
					return !PEqualityComparer<T>.Default.Equals(source, target);
			}
		}
	}
}
