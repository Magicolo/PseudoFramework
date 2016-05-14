using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Pooling.Internal
{
	public class MethodInitializer<T> : DefaultInitializer<T> where T : class
	{
		readonly Action<T> method;

		public MethodInitializer(Action<T> method)
		{
			this.method = method;
		}

		public override void OnCreate(T instance)
		{
			method(instance);

			base.OnCreate(instance);
		}
	}
}
