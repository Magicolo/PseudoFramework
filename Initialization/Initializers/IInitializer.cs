using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Initialization
{
	public interface IInitializer<T> : IInitializer
	{
		IInitialization<T> Cache(T reference);
		void Initialize(ref T instance, T reference);
		void Initialize(ref T instance, T reference, HashSet<object> toIgnore);
	}

	public interface IInitializer
	{
		IInitializationOperation[] CreateOperations(object reference);
		void Initialize(ref object instance, object reference);
		void Initialize(ref object instance, object reference, HashSet<object> toIgnore);
	}
}
