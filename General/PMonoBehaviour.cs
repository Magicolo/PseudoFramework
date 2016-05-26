using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Pseudo
{
	public abstract partial class PMonoBehaviour : MonoBehaviour
	{
		readonly Lazy<GameObject> cachedGameObject;
		public GameObject CachedGameObject { get { return cachedGameObject; } }

		readonly Lazy<Transform> cachedTransform;
		public Transform CachedTransform { get { return cachedTransform; } }

		protected PMonoBehaviour()
		{
			cachedGameObject = new Lazy<GameObject>(() => gameObject);
			cachedTransform = new Lazy<Transform>(() => transform);
		}

		protected virtual void OnValidate()
		{
#if UNITY_EDITOR
			Editor.Internal.InspectorUtility.OnValidate(this);
#endif
		}
	}
}

