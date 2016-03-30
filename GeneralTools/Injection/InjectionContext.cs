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
			AutoProperty = 1 << 5 | Property,
			EmptyMethod = 1 << 6 | Method,
			EmptyStructConstructor = 1 << 7,
			Parameter = 1 << 8,
		}

		public IBinder Binder;
		public ContextTypes ContextType;
		public object Instance;
		public Type ContractType;
		public MemberInfo Member;
		public ParameterInfo Parameter;
		public InjectAttribute Attribute;

		public Type DeclaringType
		{
			get { return Member == null ? null : Member.DeclaringType; }
		}
		public string Identifier
		{
			get { return Attribute == null ? string.Empty : Attribute.Identifier; }
		}
		public bool Optional
		{
			get { return Attribute == null ? false : Attribute.Optional; }
		}

		public override string ToString()
		{
			return string.Format("{0}(ContextType: {1}, ContractType: {2}, Member: {3}, Parameter: {4}, Attribute: {5})", GetType().Name, ContextType, ContractType, Member, Parameter, Attribute);
		}
	}
}
