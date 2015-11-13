using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Pseudo
{
	public abstract class PMonoBehaviour : MonoBehaviour, IPoolable
	{
		public bool IsInPool { get { return isInPool; } }

		bool isInPool;
		bool initialized;

		bool isGameObjectCached;
		GameObject gameObjectCached;
		public GameObject CachedGameObject
		{
			get
			{
				if (!isGameObjectCached)
				{
					isGameObjectCached = true;
					gameObjectCached = gameObject;
				}

				return gameObjectCached;
			}
		}

		bool isTransformCached;
		Transform transformCached;
		public Transform CachedTransform
		{
			get
			{
				if (!isTransformCached)
				{
					isTransformCached = true;
					transformCached = transform;
				}

				return transformCached;
			}
		}

		protected virtual void Awake()
		{
			if (!initialized)
				OnCreate();
		}

		public virtual void OnCreate()
		{
			isInPool = false;
			initialized = true;
		}

		public virtual void OnRecycle()
		{
			isInPool = true;
		}
	}
}

