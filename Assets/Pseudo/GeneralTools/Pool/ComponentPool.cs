using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal;

namespace Pseudo
{
	public class ComponentPool<T> : PrefabPool<T> where T : Component
	{
		public ComponentPool(T prefab, int startCount = 0) : base(prefab, startCount) { }

		protected override GameObject GetGameObject(T item)
		{
			return item.gameObject;
		}

		protected override Transform GetTransform(T item)
		{
			return item.transform;
		}
	}
}
