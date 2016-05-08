using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Initialization.Internal
{
	public class PureInitializer<T> : Initializer<T>
	{
		protected override bool PreInitialize(ref T instance, T reference, HashSet<object> toIgnore)
		{
			return true;
		}

		protected override bool PreInitialize(ref object instance, object reference, HashSet<object> toIgnore)
		{
			return true;
		}

		protected override void Initialize(ref T instance, T reference, HashSet<object> toIgnore)
		{
			instance = reference;
		}

		protected override void Initialize(ref object instance, object reference, HashSet<object> toIgnore)
		{
			instance = reference;
		}
	}
}
