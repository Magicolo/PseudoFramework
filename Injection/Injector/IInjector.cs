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
		IInjectorSelector InjectorSelector { get; set; }

		void Inject(InjectionContext context);
	}
}
