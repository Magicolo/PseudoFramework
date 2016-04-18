using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Injection
{
	public interface IInstantiator
	{
		IContainer Container { get; }
		IConstructorSelector ConstructorSelector { get; set; }

		object Instantiate(InjectionContext context);
		bool CanInstantiate(InjectionContext context);
	}
}
