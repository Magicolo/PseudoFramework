using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal.Injection
{
	public class FactoryData
	{
		static readonly Predicate<InjectionContext> defaultCondition = c => string.IsNullOrEmpty(c.Identifier);

		public readonly IInjectionFactory Factory;
		public Predicate<InjectionContext> Condition = defaultCondition;

		public FactoryData(IInjectionFactory factory)
		{
			Factory = factory;
		}
	}
}
