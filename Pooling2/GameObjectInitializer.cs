using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Pooling2.Internal
{
	public class GameObjectInitializer : Initializer<GameObject>
	{
		public override void OnCreate(GameObject instance)
		{
			instance.BroadcastMessage("OnCreate");
			instance.SetActive(true);
		}

		public override void OnRecycle(GameObject instance)
		{
			instance.SetActive(false);
			instance.BroadcastMessage("OnRecycle");
		}
	}
}
