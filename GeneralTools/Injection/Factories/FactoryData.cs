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
		public readonly IInjectionFactory Factory;
		public Predicate<InjectionContext> Condition = c => string.IsNullOrEmpty(c.Identifier);

		public FactoryData(IInjectionFactory factory)
		{
			Factory = factory;
		}
	}
}
