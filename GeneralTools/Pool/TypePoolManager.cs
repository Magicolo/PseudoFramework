using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo.Internal
{
	public abstract class TypePoolManager<T, TPool> : PoolManagerBase<T, Type, Type, TPool> where T : class where TPool : PoolBase<T>
	{
		public virtual TD Create<TD>() where TD : class, T
		{
			TPool pool = GetPool(typeof(TD));
			TD item = (TD)pool.Create();

			return item;
		}

		public virtual TC CreateCopy<TC>(TC reference) where TC : class, T, ICopyable<TC>
		{
			TPool pool = GetPool(reference.GetType());
			TC item = pool.CreateCopy(reference);

			return item;
		}

		public virtual void CreateElements<TC>(IList<TC> array) where TC : class, T, ICopyable<TC>
		{
			if (array == null)
				return;

			for (int i = 0; i < array.Count; i++)
				array[i] = CreateCopy(array[i]);
		}

		public override void Recycle(T item)
		{
			if (item == null)
				return;

			TPool pool = GetPool(item.GetType());
			pool.Recycle(item);
		}

		protected override Type GetPoolKey(Type identifier)
		{
			return identifier;
		}
	}
}