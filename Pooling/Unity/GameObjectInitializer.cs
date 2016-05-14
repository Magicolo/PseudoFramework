using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Pooling.Internal
{
	public class GameObjectInitializer : Initializer<GameObject>
	{
		readonly Transform transform;

		public GameObjectInitializer(Transform transform)
		{
			this.transform = transform;
		}

		public override void OnCreate(GameObject instance)
		{
			instance.transform.parent = null;
			instance.BroadcastMessage("OnCreate");
			instance.SetActive(true);
		}

		public override void OnRecycle(GameObject instance)
		{
			instance.SetActive(false);
			instance.BroadcastMessage("OnRecycle");
			instance.transform.parent = transform;
		}
	}
}
