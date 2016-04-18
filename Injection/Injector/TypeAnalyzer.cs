using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Injection.Internal
{
	public class TypeAnalyzer : ITypeAnalyzer
	{
		readonly Dictionary<Type, ITypeInfo> typeToInjectionInfo = new Dictionary<Type, ITypeInfo>();

		public ITypeInfo Analyze(Type type)
		{
			ITypeInfo info;

			if (!typeToInjectionInfo.TryGetValue(type, out info))
			{
				info = new TypeInfo(type);
				typeToInjectionInfo[type] = info;
			}

			return info;
		}
	}
}
