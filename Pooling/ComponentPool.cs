using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo.Pooling
{
	public class ComponentPool<T> : Pool<T> where T : Component
	{
		public readonly Transform Transform;

		public ComponentPool(T reference, Transform transform, int startSize) :
			base(reference, reference.GetType(), null, null, startSize, false)
		{
			Transform = transform;
			Initialize();
		}

		public override T Create()
		{
			var instance = base.Create();
			instance.transform.Copy(((T)reference).transform);

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
			var component = (T)instance;
			component.gameObject.SetActive(false);
			component.transform.parent = Transform;

			base.Enqueue(instance, initialize);
		}

		protected override object GetInstance()
		{
			var instance = (T)base.GetInstance();
			instance.gameObject.SetActive(true);

			return instance;
		}

		protected override object Construct()
		{
			var instance = UnityEngine.Object.Instantiate((T)reference);
			instance.transform.parent = Transform;
			instance.gameObject.SetActive(true);

			return instance;
		}

		protected override void Destroy(object instance)
		{
			((T)instance).gameObject.Destroy();
		}
	}
}