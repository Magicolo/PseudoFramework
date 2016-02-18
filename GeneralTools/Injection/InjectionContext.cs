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
		[Flags]
		public enum ContextTypes
		{
			None = 0,
			Member = 1 << 0,
			Field = 1 << 1 | Member,
			Property = 1 << 2 | Member,
			Method = 1 << 3 | Member,
			Constructor = 1 << 4 | Member,
			Parameter = 1 << 5,
			MethodParameter = 1 << 6 | Method | Parameter,
			ConstructorParameter = 1 << 7 | Constructor | Parameter
		}

		public IBinder Binder;
		public ContextTypes ContextType;
		public object Instance;
		public Type ContractType;
		public Type DeclaringType;
		public MemberInfo Member;
		public ParameterInfo Parameter;
		public bool Optional;
		public string Identifier;

		public override string ToString()
		{
			return string.Format("{0}(ContextType: {1}, ContractType: {2}, DeclaringType: {3}, Member: {4}, Parameter: {5}, Identifier: {6})", GetType().Name, ContextType, ContractType, DeclaringType, Member, Parameter, Identifier);
		}
	}
}
