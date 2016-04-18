using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;

namespace Pseudo.Injection
{
	[Flags]
	public enum ContextTypes
	{
		None = 0,
		Member = 1 << 0,
		Field = 1 << 1 | Member,
		Property = 1 << 2 | Member,
		Method = 1 << 3 | Member,
		Constructor = 1 << 4 | Member,
		AutoProperty = 1 << 5 | Property,
		EmptyMethod = 1 << 6 | Method,
		EmptyStructConstructor = 1 << 7,
		Parameter = 1 << 8,
	}

	public struct InjectionContext
	{
		public IContainer Container;
		public IInjectableElement Element;
		public ContextTypes Type;
		public object Instance;
		public object[] Arguments;
		public Type ContractType;
		public Type DeclaringType;
		public string Identifier;
		public bool Optional;

		public override string ToString()
		{
			return string.Format("{0}(ContextType: {1}, ContractType: {2}, DeclaringType: {3})", GetType().Name, Type, ContractType, DeclaringType);
		}
	}
}
