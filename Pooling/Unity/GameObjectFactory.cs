using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Pooling.Internal
{
	public class GameObjectFactory : PrefabFactory<GameObject>
	{
		readonly Transform transform;

		public GameObjectFactory(GameObject prefab, Transform transform) : base(prefab)
		{
			this.transform = transform;
		}

		public override GameObject Create()
		{
			var instance = base.Create();
			instance.SetActive(false);
			instance.transform.parent = transform;

			return instance;
		}
	}
}
