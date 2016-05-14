using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Pooling.Internal
{
	public class DefaultInitializer<T> : Initializer<T>
	{
		public override void OnCreate(T instance)
		{
			var poolable = instance as IPoolable;

			if (poolable != null)
				poolable.OnCreate();
		}

		public override void OnRecycle(T instance)
		{
			var poolable = instance as IPoolable;

			if (poolable != null)
				poolable.OnRecycle();
		}
	}
}
