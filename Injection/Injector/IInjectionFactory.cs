using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Injection
{
	public interface IInjectionFactory
	{
		object Create(InjectionContext context);
	}

	public interface IInjectionFactory<T> : IInjectionFactory
	{
		new T Create(InjectionContext context);
	}
}
