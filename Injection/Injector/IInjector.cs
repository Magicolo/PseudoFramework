using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Injection
{
	public interface IInjector
	{
		IContainer Container { get; }

		void Inject(InjectionContext context);
		void Inject(object instance);
	}
}
