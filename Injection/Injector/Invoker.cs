using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Injection.Internal
{
	public class Invoker<T> : InvokerBase
	{
		readonly Action<T> action;

		public Invoker(Action<T> action)
		{
			this.action = action;
		}

		public override void Invoke(object instance, params object[] arguments)
		{
			action((T)instance);
		}
	}
}
