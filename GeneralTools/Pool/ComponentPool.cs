using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo.Internal.Pool
{
	public class ComponentPool<T> : ComponentPool where T : Component
	{
		public ComponentPool(T reference, int startSize = 4) : base(reference, startSize) { }

		new public T Create()
		{
			return (T)base.Create();
		}
	}

	public class ComponentPool : PrefabPool
	{
		public GameObject GameObject { get { return gameObject == null ? (gameObject = new GameObject(((UnityEngine.Object)reference).name + " Pool")) : gameObject; } }
		public Transform Transform { get { return transform == null ? (transform = GameObject.transform) : transform; } }

		protected GameObject gameObject;
		protected Transform transform;

		public ComponentPool(Component reference, int startSize = 4) : base(reference, startSize) { }

		public override object Create()
		{
			var instance = (Component)base.Create();
			instance.transform.Copy(((Component)reference).transform);

			return instance;
		}

		public override void Clear()
		{
			lock (instances)
			{
				while (instances.Count > 0)
				{
					var instance = (Component)instances.Dequeue();

					if (instance != null)
						instance.gameObject.Destroy();
				}
			}

			lock (toInitialize)
			{
				while (toInitialize.Count > 0)
				{
					var instance = (Component)toInitialize.Dequeue();

					if (instance != null)
						instance.gameObject.Destroy();
				}
			}

			Pseudo.ObjectExtensions.Destroy(GameObject);
		}

		protected override object GetInstance()
		{
			var instance = (Component)base.GetInstance();
			instance.gameObject.SetActive(true);

			return instance;
		}

		protected override void Enqueue(object instance, bool initialize)
		{
			base.Enqueue(instance, initialize);

			var component = (Component)instance;
			component.gameObject.SetActive(false);
			component.transform.parent = Transform;
		}

		protected override object Dequeue()
		{
			var instance = (Component)base.Dequeue();
			instance.gameObject.SetActive(true);

			return instance;
		}
	}
}