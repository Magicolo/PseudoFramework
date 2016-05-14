using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Pooling.Internal
{
	public class ComponentFactory<T> : PrefabFactory<T> where T : UnityEngine.Object
	{
		readonly Transform transform;

		public ComponentFactory(T prefab, Transform transform) : base(prefab)
		{
			this.transform = transform;
		}

		public override T Create()
		{
			var instance = base.Create();
			var gameObject = instance.GetGameObject();
			gameObject.SetActive(false);
			gameObject.transform.parent = transform;

			return instance;
		}
	}
}
