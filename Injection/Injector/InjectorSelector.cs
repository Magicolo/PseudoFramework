using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Injection.Internal
{
	public class InjectorSelector : IInjectorSelector
	{
		readonly IMemberInjector injector = new MemberInjector();

		public IMemberInjector Select(InjectionContext context, ITypeInfo info)
		{
			return injector;
		}
	}
}
