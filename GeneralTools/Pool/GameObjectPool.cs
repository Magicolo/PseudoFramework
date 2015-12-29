using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal;

namespace Pseudo.Internal.Pool
{
	public class GameObjectPool : PrefabPool
	{
		public GameObject GameObject { get { return gameObject == null ? (gameObject = new GameObject(((UnityEngine.Object)reference).name + " Pool")) : gameObject; } }
		public Transform Transform { get { return transform == null ? (transform = GameObject.transform) : transform; } }

		protected GameObject gameObject;
		protected Transform transform;

		public GameObjectPool(GameObject reference, int startSize) : base(reference, startSize) { }

		new public GameObject Create()
		{
			var instance = (GameObject)base.Create();
			instance.transform.Copy(((GameObject)reference).transform);

			return instance;
		}

		protected override object CreateInstance()
		{
			var instance = (GameObject)base.CreateInstance();
			instance.SetActive(true);
			instance.transform.parent = Transform;

			return instance;
		}

		public override void Clear()
		{
			base.Clear();

			Pseudo.ObjectExtensions.Destroy(GameObject);
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