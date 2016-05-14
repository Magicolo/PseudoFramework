using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Pooling.Internal;

namespace Pseudo.Pooling
{
	// Should not be generalized because 'Default' scans for all 'IInitializer<T>' which means
	// that user might have defined an IInitializer<T> for a different module.
	public abstract class Initializer<T> : IInitializer<T>
	{
		public static IInitializer<T> Default
		{
			get
			{
				if (defaultInitializer == null)
					defaultInitializer = PoolingUtility.CreateInitializer<T>();

				return defaultInitializer;
			}
		}

		static IInitializer<T> defaultInitializer;

		public abstract void OnCreate(T instance);
		public abstract void OnRecycle(T instance);

		void IInitializer.OnCreate(object instance)
		{
			OnCreate((T)instance);
		}

		void IInitializer.OnRecycle(object instance)
		{
			OnRecycle((T)instance);
		}
	}
}
