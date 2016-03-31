using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Injection
{
	public interface IInstantiator
	{
		IBinder Binder { get; }

		object Instantiate(Type concreteType);
		T Instantiate<T>();
	}
}
