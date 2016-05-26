using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.PoolingNOOOO.Internal
{
	public abstract class PoolingUpdaterBase : IPoolingUpdater
	{
		public IPool Pool { get; private set; }
		public bool Updating { get; protected set; }

		protected readonly Queue<object> instances = new Queue<object>();
		protected readonly Queue<object> toInitialize = new Queue<object>();

		protected PoolingUpdaterBase(IPool pool)
		{
			Pool = pool;
		}

		public abstract void Enqueue(object instance);
		public abstract object Dequeue();
		public abstract void Clear();
		public abstract void Reset();
	}
}
