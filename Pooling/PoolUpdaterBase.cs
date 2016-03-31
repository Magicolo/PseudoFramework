using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Pooling.Internal
{
	public abstract class PoolUpdaterBase : IPoolUpdater
	{
		public bool Updating { get; protected set; }
		public abstract IFieldInitializer Initializer { get; set; }

		protected readonly Queue<object> instances;
		protected readonly Queue<object> toInitialize;
		protected IFieldInitializer initializer;

		protected PoolUpdaterBase()
		{
			this.instances = new Queue<object>();
			this.toInitialize = new Queue<object>();
		}

		public abstract void Update();
		public abstract void Enqueue(object instance, bool initialize);
		public abstract object Dequeue();
		public abstract void Clear();
		public abstract void Reset();
	}
}
