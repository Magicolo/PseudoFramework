using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal;

namespace Pseudo
{
	public class ComponentPoolz<T> : PrefabPoolz<T> where T : Component
	{
		public ComponentPoolz(T prefab, int startCount = 4) : base(prefab, startCount) { }

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
