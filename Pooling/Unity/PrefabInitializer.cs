using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Pooling.Internal
{
	public class PrefabInitializer<T> : Initializer<T> where T : UnityEngine.Object
	{
		public override void OnCreate(T instance)
		{
			instance.GetGameObject().BroadcastMessage("OnCreate");
		}

		public override void OnRecycle(T instance)
		{
			instance.GetGameObject().BroadcastMessage("OnRecycle");
		}
	}
}
