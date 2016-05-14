using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Pooling.Internal
{
	public class MethodFactory<T> : FactoryBase<T>
	{
		readonly Func<T> method;

		public MethodFactory(Func<T> method)
		{
			this.method = method;
		}

		public override T Create()
		{
			return method();
		}
	}
}
