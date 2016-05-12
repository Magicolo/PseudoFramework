using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Pooling2.Internal;

namespace Pseudo.Pooling2
{
	public abstract class Initializer<T> : IInitializer<T> where T : class
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
			OnCreate(instance as T);
		}

		void IInitializer.OnRecycle(object instance)
		{
			OnRecycle(instance as T);
		}
	}
}
