using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Injection.Internal
{
	public class Binding : IBinding
	{
		static readonly Predicate<InjectionContext> defaultCondition = c => true;

		public Type ContractType { get; private set; }
		public IInjectionFactory Factory { get; set; }
		public Predicate<InjectionContext> Condition
		{
			get { return condition; }
			set { condition = value ?? defaultCondition; }
		}

		Predicate<InjectionContext> condition;

		public Binding(Type contractType, IInjectionFactory factory)
			: this(contractType, factory, null) { }

		public Binding(Type contractType, IInjectionFactory factory, Predicate<InjectionContext> condition)
		{
			ContractType = contractType;
			Factory = factory;
			Condition = condition;
		}

		public override string ToString()
		{
			return string.Format("{0}({1}, {2})", GetType().Name, ContractType.Name, Factory);
		}
	}
}
