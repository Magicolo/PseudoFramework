using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Injection
{
	public interface IMemberInjector
	{
		void Inject(InjectionContext context, ITypeInfo info);
	}
}
