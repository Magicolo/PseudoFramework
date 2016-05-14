using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Reflection;

namespace Pseudo.Pooling.Internal
{
	public class DefaultFactory<T> : FactoryBase<T>
	{
		static readonly IConstructorWrapper constructor = ReflectionUtility.CreateDefaultConstructorWrapper(typeof(T));

		public override T Create()
		{
			return (T)constructor.Invoke();
		}
	}
}
