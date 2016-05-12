using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Pooling2.Internal
{
	public static class PoolingUtility
	{
		static Type[] initializerTypes = TypeUtility.AllTypes
		   .Where(t => t.Is<IInitializer>() && t.IsConcrete() && t.HasDefaultConstructor())
		   .ToArray();

		public static IInitializer<T> CreateInitializer<T>() where T : class
		{
			var initializerType = Array.Find(initializerTypes, t => t.Is<IInitializer<T>>());

			if (initializerType == null)
			{
				if (typeof(T).Is<IPoolable>())
					initializerType = typeof(GenericInitializer<>).MakeGenericType(typeof(T));
				else if (typeof(T).Is<Component>())
					initializerType = typeof(ComponentInitializer<>).MakeGenericType(typeof(T));
				else
					return new EmptyInitializer<T>();
			}

			return (IInitializer<T>)Activator.CreateInstance(initializerType);
		}
	}
}
