using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Injection
{
	public interface IInjectionFactory : IFactory<InjectionContext, object> { }

	public interface IInjectionFactory<out T> : IInjectionFactory
	{
		new T Create(InjectionContext context);
	}
}
