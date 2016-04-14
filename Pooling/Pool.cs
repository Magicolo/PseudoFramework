using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal;
using System;
using Pseudo.Pooling.Internal;

namespace Pseudo.Pooling
{
	public delegate T Constructor<out T>();
	public delegate object Constructor();
	public delegate void Destructor<T>(T instance);
	public delegate void Destructor(object instance);

	public class Pool<T> : PoolBase, IPool<T> where T : class
	{
		public Pool(T reference, int startSize) :
			this(reference, reference.GetType(), null, null, startSize, true)
		{ }

		public Pool(T reference, Constructor<T> constructor, int startSize) :
			this(reference, reference.GetType(), () => constructor(), null, startSize, true)
		{ }

		public Pool(T reference, Constructor<T> constructor, Destructor<T> destructor, int startSize) :
			this(reference, reference.GetType(), () => constructor(), instance => destructor((T)instance), startSize, true)
		{ }

		protected Pool(object reference, Type type, Constructor constructor, Destructor destructor, int startSize, bool initialize) :
			base(reference, type, constructor, destructor, startSize, initialize)
		{ }

		new public virtual T Create()
		{
			return (T)base.CreateObject();
		}

		public void Recycle(T instance)
		{
			Recycle((object)instance);
		}

		public void RecycleElements(IList<T> elements)
		{
			Recycle((IList)elements);
		}

		public bool Contains(T instance)
		{
			return Contains((object)instance);
		}

		protected override object CreateObject()
		{
			return Create();
		}
	}
}