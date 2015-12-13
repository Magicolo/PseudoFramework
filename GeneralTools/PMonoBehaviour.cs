using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Pseudo
{
	public abstract class PMonoBehaviour : MonoBehaviour, IPoolable
	{
		[DoNotInitialize]
		bool created;

		readonly CachedValue<GameObject> cachedGameObject;
		public GameObject CachedGameObject { get { return cachedGameObject; } }

		readonly CachedValue<Transform> cachedTransform;
		public Transform CachedTransform { get { return cachedTransform; } }

		protected PMonoBehaviour()
		{
			cachedGameObject = new CachedValue<GameObject>(() => gameObject);
			cachedTransform = new CachedValue<Transform>(() => transform);
		}

		protected virtual void Start()
		{
			if (!created)
				OnCreate();
		}

		public virtual void OnCreate() { created = true; }
		public virtual void OnRecycle() { }
	}
}

