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
		object Instantiate(Type concreteType, params object[] additional);
		T Instantiate<T>(params object[] additional) where T : class;
	}
}
