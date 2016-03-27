using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal.Injection
{
	public abstract class InvokerBase
	{
		static readonly object[] emptyArguments = new object[0];

		public virtual void Invoke(object instance)
		{
			Invoke(instance, emptyArguments);
		}

		public abstract void Invoke(object instance, params object[] arguments);
	}
}
