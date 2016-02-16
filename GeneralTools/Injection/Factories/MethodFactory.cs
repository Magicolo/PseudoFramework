using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal.Injection
{
	public class MethodFactory : IFactory
	{
		readonly IBinder binder;
		readonly InjectionMethod<object> method;

		public MethodFactory(IBinder binder, InjectionMethod<object> method)
		{
			this.binder = binder;
			this.method = method;
		}

		public object Create(params object[] arguments)
		{
			return method(binder, arguments);
		}
	}
}
