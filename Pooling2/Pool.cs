using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Pooling2.Internal;

namespace Pseudo.Pooling2
{
	public class Pool<T> : PoolBase<T> where T : class
	{
		readonly Func<T> construct;
		readonly Action<T> initialize;

		public Pool(Func<T> construct, Action<T> initialize)
		{
			this.construct = construct;
			this.initialize = initialize;
		}

		protected override T Construct()
		{
			return construct();
		}

		protected override void Initialize(T instance)
		{
			initialize(instance);
		}
	}
}
