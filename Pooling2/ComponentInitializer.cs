using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Pooling2.Internal
{
	public class ComponentInitializer<T> : Initializer<T> where T : Component
	{
		public override void OnCreate(T instance)
		{
			instance.gameObject.BroadcastMessage("OnCreate");
			instance.gameObject.SetActive(true);
		}

		public override void OnRecycle(T instance)
		{
			instance.gameObject.SetActive(false);
			instance.gameObject.BroadcastMessage("OnRecycle");
		}
	}
}
