using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal;

namespace Pseudo
{
	public class PoolManager<T> : TypePoolManager<T, Pool<T>> where T : class, IPoolable
	{
		protected readonly Func<Type, Pool<T>> createPool;

		public PoolManager(Func<Type, Pool<T>> createPool)
		{
			this.createPool = createPool;
		}

		protected override Pool<T> CreatePool(Type identifier)
		{
			return createPool(identifier);
		}
	}
}
