using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal;

namespace Pseudo.Internal.Pool
{
	public class ScriptablePool<T> : Pool<T> where T : ScriptableObject
	{
		public ScriptablePool(int startSize) : this(ScriptableObject.CreateInstance<T>(), startSize) { }

		public ScriptablePool(T reference, int startSize) : base(reference, startSize) { }

		protected override object Construct()
		{
			return UnityEngine.Object.Instantiate((T)reference);
		}

		protected override void Destroy(object instance)
		{
			((T)instance).Destroy();
		}
	}
}