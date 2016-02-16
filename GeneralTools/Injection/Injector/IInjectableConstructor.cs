using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;

namespace Pseudo
{
	public interface IInjectableConstructor
	{
		ConstructorInfo Constructor { get; }

		object Inject(IResolver resolver, object[] additional);
		bool CanInject(IResolver resolver, object[] additional);
	}
}
