using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Injection
{
	public interface IInjectionInterceptor
	{
		void Inject(InjectionContext context, ITypeInfo info);
		bool CanInject(InjectionContext context, ITypeInfo info);
	}
}
