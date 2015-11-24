using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal;

namespace Pseudo
{
	public class GameObjectPoolz : PrefabPoolz<GameObject>
	{
		public GameObjectPoolz(GameObject prefab, int startCount = 4) : base(prefab, startCount) { }

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