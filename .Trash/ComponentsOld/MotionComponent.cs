using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal.Entity;
using Zenject;

namespace Pseudo
{
	[RequireComponent(typeof(TimeComponent))]
	public class MotionComponent : ComponentBehaviour
	{
		public EntityBehaviour Agent;
		public MinMax Speed = new MinMax(0f, 5f);
		public float Acceleration = 3f;

		readonly Lazy<Transform> cachedTransform;
		public Transform Transform { get { return cachedTransform.Value; } }

		public MotionComponent()
		{
			cachedTransform = new Lazy<Transform>(() => CachedTransform.root);
		}

		public Vector3 Velocity
		{
			get { return velocity; }
			set { velocity = value; }
		}

		Vector3 velocity;
	}
}