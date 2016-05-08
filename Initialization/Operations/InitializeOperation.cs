using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Initialization.Internal
{
	public delegate void InitializationOperation(ref object instance);

	public class InitializeOperation : IInitializationOperation
	{
		readonly object reference;
		readonly IInitializer initializer;

		public InitializeOperation(object reference, IInitializer initializer)
		{
			this.reference = reference;
			this.initializer = initializer;
		}

		public void Initialize(ref object instance, HashSet<object> toIgnore)
		{
			initializer.Initialize(ref instance, reference, toIgnore);
		}
	}
}
