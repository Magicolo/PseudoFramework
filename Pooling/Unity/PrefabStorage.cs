using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Pooling.Internal
{
	public class PrefabStorage<T> : Storage<T> where T : UnityEngine.Object
	{
		readonly Transform root;

		public PrefabStorage(Transform root, IFactory<T> factory) : base(factory)
		{
			this.root = root;
		}

		public override T Take()
		{
			var instance = base.Take();

			if (instance != null)
			{
				var gameObject = instance.GetGameObject();
				gameObject.transform.parent = null;
				gameObject.SetActive(true);
			}

			return instance;
		}

		public override bool Put(T instance)
		{
			if (base.Put(instance))
			{
				var gameObject = instance.GetGameObject();
				gameObject.SetActive(false);
				gameObject.transform.parent = root;
			}
			else if (instance != null)
				instance.GetGameObject().Destroy();

			return false;
		}
	}
}
