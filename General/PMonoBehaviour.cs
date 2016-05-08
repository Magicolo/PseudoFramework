using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using Pseudo.Injection;
using Pseudo.Pooling;

namespace Pseudo
{
	public abstract class PMonoBehaviour : MonoBehaviour
	{
		readonly Lazy<GameObject> cachedGameObject;
		public GameObject CachedGameObject { get { return cachedGameObject; } }

		readonly Lazy<Transform> cachedTransform;
		public Transform CachedTransform { get { return cachedTransform; } }

		[Inject, DoNotInitialize]
		IRoot root;
		[NonSerialized, DoNotInitialize]
		bool injected;

		protected PMonoBehaviour()
		{
			cachedGameObject = new Lazy<GameObject>(() => gameObject);
			cachedTransform = new Lazy<Transform>(() => transform);
		}

		public void Inject()
		{
			root = root ?? SceneUtility.FindComponent<SceneRoot>(CachedGameObject.scene);

			if (root == null || root.Container == null)
				return;

			root.Container.Injector.Inject(this);
			injected = true;
		}

		protected virtual void Start()
		{
			if (!injected)
				Inject();
		}

		protected virtual void OnValidate()
		{
#if UNITY_EDITOR
			Editor.Internal.InspectorUtility.OnValidate(this);
#endif
		}
	}
}

