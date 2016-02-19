using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo
{
	public interface IInstantiator
	{
		IBinder Binder { get; }

		object Instantiate(Type concreteType);
		T Instantiate<T>() where T : class;
	}
}
