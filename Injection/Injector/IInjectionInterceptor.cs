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
		void OnInject(InjectionContext context, ITypeInfo info);
	}
}
