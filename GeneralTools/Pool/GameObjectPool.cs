using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal;

namespace Pseudo
{
	public class GameObjectPool : Pool<GameObject>
	{
		public readonly Transform Transform;

		public GameObjectPool(GameObject reference, Transform transform, int startSize) : base(reference, reference.GetType(), null, null, startSize, false)
		{
			Transform = transform;
			Initialize();
		}

		public override GameObject Create()
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

			if (ApplicationUtility.IsPlaying)
				gameObject.transform.parent = Transform;
		}

		protected override object Dequeue()
		{
			var instance = (GameObject)base.Dequeue();
			instance.SetActive(true);

			return instance;
		}

		protected override object Construct()
		{
			var instance = UnityEngine.Object.Instantiate((GameObject)reference);

			if (ApplicationUtility.IsPlaying)
				instance.transform.parent = Transform;

			instance.gameObject.SetActive(true);

			return instance;
		}

		protected override void Destroy(object instance)
		{
			((GameObject)instance).Destroy();
		}
	}
}