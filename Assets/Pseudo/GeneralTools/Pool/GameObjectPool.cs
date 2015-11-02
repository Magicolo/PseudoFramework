using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal;

namespace Pseudo
{
	public class GameObjectPool : PrefabPool<GameObject>
	{
		public GameObjectPool(GameObject prefab, int startCount = 0) : base(prefab, startCount) { }

		protected override GameObject GetGameObject(GameObject item)
		{
			return item;
		}

		protected override Transform GetTransform(GameObject item)
		{
			return item.transform;
		}
	}
}