using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;

namespace Pseudo
{
	public struct InjectionContext
	{
		public enum Types
		{
			Field,
			Property,
			Method,
			Constructor
		}

		public IResolver Resolver;
		public object[] Additional;
		public Types Type;
		public object Instance;
		public Type ContractType;
		public Type DeclaringType;
		public bool Optional;
		public string Identifier;

		public override string ToString()
		{
			return string.Format("{0}({1}, {2}, {3})", GetType().Name, Type, ContractType.Name, DeclaringType.Name);
		}
	}
}
