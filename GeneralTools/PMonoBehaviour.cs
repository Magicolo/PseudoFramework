using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Pseudo
{
	public abstract class PMonoBehaviour : MonoBehaviour, IPoolable
	{
		readonly CachedValue<GameObject> cachedGameObject;
		public GameObject GameObject { get { return cachedGameObject; } }

		readonly CachedValue<Transform> cachedTransform;
		public Transform Transform { get { return cachedTransform; } }

		protected PMonoBehaviour()
		{
			cachedGameObject = new CachedValue<GameObject>(() => gameObject);
			cachedTransform = new CachedValue<Transform>(() => transform);
		}

		public virtual void OnCreate() { }
		public virtual void OnRecycle() { }
	}
}

