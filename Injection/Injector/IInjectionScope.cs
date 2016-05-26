using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Injection
{
	public interface IInjectionScope
	{
		object GetInstance(IInjectionFactory factory, InjectionContext context);
	}

	public interface IInjectionScope<T> : IInjectionScope
	{
		T GetInstance(IInjectionFactory<T> factory, InjectionContext context);
	}
}
