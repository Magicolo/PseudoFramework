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
			None,
			Field,
			Property,
			Method,
			MethodParameter,
			Constructor,
			ConstructorParameter
		}

		public IBinder Binder;
		public Types Type;
		public object Instance;
		public Type ContractType;
		public Type DeclaringType;
		public MemberInfo Member;
		public ParameterInfo Parameter;
		public bool Optional;
		public string Identifier;

		public override string ToString()
		{
			return string.Format("{0}({1}, {2}, {3}, {4})", GetType().Name, Type, DeclaringType, Member, Parameter);
		}
	}
}
