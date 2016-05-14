using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Pooling.Internal
{
	public class ComponentInitializer<T> : Initializer<T> where T : UnityEngine.Object
	{
		readonly GameObjectInitializer initializer;

		public ComponentInitializer(Transform transform)
		{
			initializer = new GameObjectInitializer(transform);
		}

		public override void OnCreate(T instance)
		{
			initializer.OnCreate(instance.GetGameObject());
		}

		public override void OnRecycle(T instance)
		{
			initializer.OnRecycle(instance.GetGameObject());
		}
	}
}
