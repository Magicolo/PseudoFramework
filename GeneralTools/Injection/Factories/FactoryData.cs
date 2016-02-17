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
		public IInjectionFactory Factory;
		public List<Predicate<InjectionContext>> Conditions;
	}
}
