using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Initialization.Internal
{
	public class Initialization<T> : IInitialization<T>
	{
		readonly IInitializationOperation[] operations;
		readonly HashSet<object> toIgnore = new HashSet<object>();

		public Initialization(params IInitializationOperation[] operations)
		{
			this.operations = operations;
		}

		public void Initialize(ref T instance)
		{
			object boxedInstance = instance;
			Initialize(ref boxedInstance);
			instance = (T)boxedInstance;
		}

		public void Initialize(ref object instance)
		{
			toIgnore.Clear();

			for (int i = 0; i < operations.Length; i++)
				operations[i].Initialize(ref instance, toIgnore);
		}
	}
}
