using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Pseudo;
using Pseudo.Internal;

namespace Pseudo
{
	public class Pool<T> : PoolBase<T> where T : class, IPoolable
	{
		protected readonly Func<T> getNewItem;

		public Pool(Func<T> getNewItem, int startCount = 4) : base(startCount)
		{
			this.getNewItem = getNewItem;

			Initialize();
		}

		public override T Create()
		{
			T item = base.Create();
			item.OnCreate();

			return item;
		}

		public override TC CreateCopy<TC>(TC reference)
		{
			TC item = base.CreateCopy(reference);
			item.OnCreate();

			return item;
		}

		public override void Recycle(T item)
		{
			base.Recycle(item);
			item.OnRecycle();
		}

		protected override T CreateItem()
		{
			return getNewItem();
		}
	}
}