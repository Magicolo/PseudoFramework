using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Pooling2.Internal
{
	public class GenericInitializer<T> : Initializer<T> where T : class, IPoolable
	{
		public override void OnCreate(T instance)
		{
			instance.OnCreate();
		}

		public override void OnRecycle(T instance)
		{
			instance.OnRecycle();
		}
	}
}
