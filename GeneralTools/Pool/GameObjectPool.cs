using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal;

namespace Pseudo.Internal.Pool
{
	public class GameObjectPool : Pool<GameObject>
	{
		public readonly Transform Transform;

		public GameObjectPool(GameObject reference, Transform transform, int startSize) :
			base(reference, () =>
			{
				var instance = UnityEngine.Object.Instantiate(reference);
				instance.transform.parent = transform;
				instance.gameObject.SetActive(true);

				return instance;
			},
				instance => ((GameObject)instance).Destroy(), startSize)
		{
			Transform = transform;
		}

		new public GameObject Create()
		{
			var instance = base.Create();
			instance.transform.Copy(((GameObject)reference).transform);

			return instance;
		}

		public override void Clear()
		{
			base.Clear();

			if (Transform != null)
				Transform.gameObject.Destroy();
		}

		protected override void Enqueue(object instance, bool initialize)
		{
			base.Enqueue(instance, initialize);

			var gameObject = (GameObject)instance;
			gameObject.SetActive(false);
			gameObject.transform.parent = Transform;
		}

		protected override object Dequeue()
		{
			var instance = (GameObject)base.Dequeue();
			instance.SetActive(true);

			return instance;
		}
	}
}