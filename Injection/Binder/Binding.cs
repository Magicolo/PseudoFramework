using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Injection.Internal
{
	public class Binding : IBinding
	{
		static readonly IInjectionScope defaultScope = new TransientScope();
		static readonly Predicate<InjectionContext> defaultCondition = c => true;

		public Type ContractType { get; private set; }
		public Type[] BaseTypes { get; private set; }
		public IInjectionFactory Factory { get; set; }
		public IInjectionScope Scope
		{
			get { return scope; }
			set { scope = value ?? defaultScope; }
		}
		public Predicate<InjectionContext> Condition
		{
			get { return condition; }
			set { condition = value ?? defaultCondition; }
		}

		IInjectionScope scope = defaultScope;
		Predicate<InjectionContext> condition = defaultCondition;

		public Binding(Type contractType, IInjectionFactory factory, IInjectionScope scope = null, Predicate<InjectionContext> condition = null)
			: this(contractType, Type.EmptyTypes, factory, scope, condition) { }

		public Binding(Type contractType, Type[] baseTypes, IInjectionFactory factory, IInjectionScope scope = null, Predicate<InjectionContext> condition = null)
		{
			ContractType = contractType;
			BaseTypes = baseTypes;
			Factory = factory;
			Scope = scope;
			Condition = condition;
		}

		public override string ToString()
		{
			return string.Format("{0}({1}, {2}, {3})", GetType().Name, ContractType.Name, Factory, Scope);
		}
	}
}
