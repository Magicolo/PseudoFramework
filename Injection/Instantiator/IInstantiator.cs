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
		IConstructorSelector Selector { get; set; }

		object Instantiate(InjectionContext context);
		object Instantiate(Type concreteType);
		T Instantiate<T>();
		bool CanInstantiate(InjectionContext context);
		bool CanInstantiate(Type concreteType);
		bool CanInstantiate<T>();
	}
}
