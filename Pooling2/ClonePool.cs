using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Pooling2.Internal;
using Pseudo.Internal;
using Pseudo.Initialization;

namespace Pseudo.Pooling2
{
	public class ClonePool<T> : PoolBase<T> where T : class
	{
		readonly T reference;
		readonly Func<T, T> construct;
		readonly IInitialization<T> initialization;

		public ClonePool(T reference)
			: this(reference, CloneExtensions.Clone) { }

		public ClonePool(T reference, Func<T, T> construct)
			: this(reference, construct, Initializer<T>.Default) { }

		public ClonePool(T reference, Func<T, T> construct, IInitializer<T> initializer)
		{
			this.reference = reference;
			this.construct = construct;

			initialization = initializer.Cache(reference);
		}

		protected override T Construct()
		{
			return construct(reference);
		}

		protected override void Initialize(T instance)
		{
			initialization.Initialize(ref instance);
		}
	}
}
