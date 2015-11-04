using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Pseudo
{
	public abstract class PMonoBehaviour : MonoBehaviour, IPoolable
	{
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

		bool isInPool;
		public bool IsInPool { get { return isInPool; } }

		public virtual void OnCreate()
		{
			isInPool = false;
		}

		public virtual void OnRecycle()
		{
			isInPool = true;
		}
	}
}

