using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Pooling.Internal
{
	public static class PoolingUtility
	{
		static Type[] initializerTypes = TypeUtility.AllTypes
		   .Where(t => t.Is<IInitializer>() && t.IsConcrete() && t.HasEmptyConstructor())
		   .ToArray();

		public static IInitializer<T> CreateInitializer<T>()
		{
			var initializerType = Array.Find(initializerTypes, t => t.Is<IInitializer<T>>());

			if (initializerType == null)
				return new DefaultInitializer<T>();

			return (IInitializer<T>)Activator.CreateInstance(initializerType);
		}
	}
}
