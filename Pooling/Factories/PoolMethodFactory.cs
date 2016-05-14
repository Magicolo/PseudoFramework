using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Pooling.Internal
{
	public class PoolMethodFactory : PoolFactoryBase
	{
		readonly Func<Type, IPool> method;

		public PoolMethodFactory(Func<Type, IPool> method)
		{
			this.method = method;
		}

		public override IPool Create(Type argument)
		{
			return method(argument);
		}
	}
}
