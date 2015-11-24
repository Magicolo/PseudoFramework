using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo
{
	public class BehaviourPoolz<T> : ComponentPoolz<T> where T : MonoBehaviour, IPoolable
	{
		public BehaviourPoolz(T prefab, int startCount = 4) : base(prefab, startCount) { }

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
	}
}