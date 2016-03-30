using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal.Injection
{
	public abstract class MethodFactoryBase<TConcrete> : IInjectionFactory
	{
		protected readonly Type contractType;
		protected readonly IBinder binder;
		protected readonly InjectionMethod<TConcrete> method;

		protected MethodFactoryBase(Type contractType, IBinder binder, InjectionMethod<TConcrete> method)
		{
			this.contractType = contractType;
			this.binder = binder;
			this.method = method;
		}

		public abstract object Create(InjectionContext argument);
	}
}
