using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;

namespace Pseudo.Internal.Injection
{
	public interface IInjectableParameter
	{
		MemberInfo Member { get; }
		ParameterInfo Parameter { get; }

		void Inject(InjectionContext context, object[] arguments, int index);
		bool CanInject(ref InjectionContext context);
	}
}
