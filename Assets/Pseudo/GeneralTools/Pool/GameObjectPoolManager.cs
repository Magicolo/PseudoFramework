using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal;

namespace Pseudo
{
	public class GameObjectPoolManager : PrefabPoolManager<GameObject>
	{
		protected override UnityEngine.Object GetPoolKey(GameObject identifier)
		{
			return identifier;
		}

		protected override PrefabPool<GameObject> CreatePool(GameObject identifier)
		{
			GameObjectPool pool = new GameObjectPool(identifier);
			pool.Transform.parent = cachedTransform.Value;

			return pool;
		}
	}
}