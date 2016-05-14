using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Reflection;
using System.Reflection;

namespace Pseudo.Pooling.Internal
{
	public class DefaultPoolFactory : PoolFactoryBase
	{
		static MethodInfo createMethod = typeof(DefaultPoolFactory).GetMethod("CreatePool", ReflectionUtility.StaticFlags);

		public override IPool Create(Type argument)
		{
			var method = createMethod.MakeGenericMethod(argument);

			return (IPool)method.Invoke(null, null);
		}

		static IPool<T> CreatePool<T>() where T : class
		{
			return new Pool<T>();
		}
	}
}
