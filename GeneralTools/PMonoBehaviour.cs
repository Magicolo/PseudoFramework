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

		readonly Lazy<GameObject> cachedGameObject;
		public GameObject CachedGameObject { get { return cachedGameObject; } }

		readonly Lazy<Transform> cachedTransform;
		public Transform CachedTransform { get { return cachedTransform; } }

		protected PMonoBehaviour()
		{
			cachedGameObject = new Lazy<GameObject>(() => gameObject);
			cachedTransform = new Lazy<Transform>(() => transform);
		}

		protected virtual void Start()
		{
			if (!created)
				OnCreate();
		}

		protected virtual void OnValidate()
		{
#if UNITY_EDITOR
			Pseudo.Internal.Editor.InspectorUtility.OnValidate(this);
#endif
		}

		public virtual void OnCreate() { created = true; }
		public virtual void OnRecycle() { }
	}
}

