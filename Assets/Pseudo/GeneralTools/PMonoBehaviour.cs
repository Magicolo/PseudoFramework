using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Pseudo
{
	public abstract class PMonoBehaviour : MonoBehaviour
	{
		CachedValue<GameObject> cachedGameObject;
		public GameObject GameObject { get { return cachedGameObject; } }

		CachedValue<Transform> cachedTransform;
		public Transform Transform { get { return cachedTransform; } }

		public PMonoBehaviour()
		{
			cachedGameObject = new CachedValue<GameObject>(() => base.gameObject);
			cachedTransform = new CachedValue<Transform>(() => base.transform);
		}
	}
}

