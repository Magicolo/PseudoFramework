using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Zenject;

namespace Pseudo.Internal.Pool
{
	public class ComponentPool<T> : Pool<T> where T : Component
	{
		public readonly Transform Transform;

		public ComponentPool(T reference, Transform transform, int startSize) :
			base(reference, () =>
			{
				var instance = UnityEngine.Object.Instantiate(reference);
				instance.transform.parent = transform;
				instance.gameObject.SetActive(true);

				return instance;
			},
				instance => ((T)instance).gameObject.Destroy(), startSize)
		{
			Transform = transform;
		}

		public override T Create()
		{
			var instance = (T)base.Create();
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
			base.Enqueue(instance, initialize);

			var component = (T)instance;
			component.gameObject.SetActive(false);
			component.transform.parent = Transform;
		}

		protected override object Dequeue()
		{
			var instance = (T)base.Dequeue();
			instance.gameObject.SetActive(true);

			return instance;
		}
	}
}