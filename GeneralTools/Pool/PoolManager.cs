using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal;

namespace Pseudo
{
	public class PoolManager<T> : TypePoolManager<T, Poolz<T>> where T : class, IPoolable
	{
		protected readonly Func<Type, Poolz<T>> createPool;

		public PoolManager(Func<Type, Poolz<T>> createPool)
		{
			this.createPool = createPool;
		}

		protected override Poolz<T> CreatePool(Type identifier)
		{
			return createPool(identifier);
		}
	}
}
