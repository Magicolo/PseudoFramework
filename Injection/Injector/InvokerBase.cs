using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Injection.Internal
{
	public abstract class InvokerBase
	{
		public virtual void Invoke(object instance)
		{
			Invoke(instance, InjectionUtility.EmptyObjects);
		}

		public abstract void Invoke(object instance, params object[] arguments);
	}
}
