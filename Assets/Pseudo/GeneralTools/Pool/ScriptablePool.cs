using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal;

namespace Pseudo
{
	public class ScriptablePool<T> : PoolBase<T> where T : ScriptableObject, IPoolable
	{
		protected Type type;

		public ScriptablePool(Type type, int startCount = 0) : base(startCount)
		{
			this.type = type;

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
			return (T)ScriptableObject.CreateInstance(type);
		}

		protected override void Enqueue(T item)
		{
			if (Application.isPlaying)
				base.Enqueue(item);
			else
				item.Destroy();
		}
	}
}