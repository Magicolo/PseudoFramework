using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Internal;

namespace Pseudo.Pooling
{
	public class ClonePool<T> : Pool<T> where T : class
	{
		public T Reference
		{
			get { return reference; }
		}
		public ICloner<T> Cloner
		{
			get { return cloner; }
		}
		public ICopier<T> Copier
		{
			get { return copier; }
		}

		readonly T reference;
		readonly ICloner<T> cloner;
		readonly ICopier<T> copier;

		public ClonePool(T reference, ICloner<T> cloner = null, ICopier<T> copier = null, IStorage<T> storage = null)
			: this(reference, cloner ?? Cloner<T>.Default, copier ?? Copier<T>.Default, storage, true) { }

		public ClonePool(T reference, Func<T, T> cloner, ICopier<T> copier = null, IStorage<T> storage = null)
			: this(reference, cloner == null ? null : new MethodCloner<T>(cloner), copier, storage) { }

		public ClonePool(T reference, Func<T, T> cloner, Action<T, T> copier, IStorage<T> storage = null)
			: this(reference, cloner, copier == null ? null : new MethodCopier<T>(copier), storage) { }

		ClonePool(T reference, ICloner<T> cloner, ICopier<T> copier, IStorage<T> storage, bool noNull)
		   : base(() => cloner.Clone(reference), instance => copier.CopyTo(reference, instance), storage)
		{
			this.reference = reference;
			this.cloner = cloner;
			this.copier = copier;
		}
	}
}
