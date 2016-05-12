using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Pooling2.Internal
{
	public class EmptyInitializer<T> : Initializer<T> where T : class
	{
		public override void OnCreate(T instance) { }
		public override void OnRecycle(T instance) { }
	}
}
