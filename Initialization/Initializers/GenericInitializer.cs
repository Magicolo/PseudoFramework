using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Initialization.Internal
{
	public class GenericInitializer<T> : Initializer<T> where T : IInitializable<T>
	{
		protected override void Initialize(ref T instance, T reference, HashSet<object> toIgnore)
		{
			instance.Initialize(reference);
		}
	}
}
