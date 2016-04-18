using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Injection
{
	public interface IContainer
	{
		IContainer Parent { get; }
		IBinder Binder { get; }
		IResolver Resolver { get; }
		IInjector Injector { get; }
		IInstantiator Instantiator { get; }
		ITypeAnalyzer Analyzer { get; set; }

		object Get(InjectionContext context);
	}
}
