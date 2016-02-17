using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal.Injection
{
	public abstract class MethodFactoryBase : IInjectionFactory
	{
		protected readonly Type contractType;
		protected readonly IBinder binder;
		protected readonly InjectionMethod<object> method;

		protected MethodFactoryBase(Type contractType, IBinder binder, InjectionMethod<object> method)
		{
			this.contractType = contractType;
			this.binder = binder;
			this.method = method;
		}

		public abstract object Create(InjectionContext argument);
	}
}
