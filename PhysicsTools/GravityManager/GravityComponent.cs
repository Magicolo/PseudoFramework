using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal.Physics;
using Pseudo.Internal.Entity;

namespace Pseudo
{
	[AddComponentMenu("Pseudo/Physics/Gravity")]
	[RequireComponent(typeof(TimeComponent))]
	public class GravityComponent : PMonoBehaviour, IGravityChannel, IComponent
	{
		public GravityManager.GravityChannels Channel
		{
			get { return gravity.Channel; }
		}
		public Vector3 Gravity
		{
			get { return gravity.Gravity; }
		}
		public Vector2 Gravity2D
		{
			get { return gravity.Gravity2D; }
		}
		public float GravityScale
		{
			get { return gravity.GravityScale; }
			set { gravity.GravityScale = value; }
		}
		public Vector3 Rotation
		{
			get { return gravity.Rotation; }
			set { gravity.Rotation = value; }
		}

		protected readonly CachedValue<Rigidbody> cachedRigidbody;
		public Rigidbody CachedRigidbody { get { return cachedRigidbody; } }

		protected readonly CachedValue<Rigidbody2D> cachedRigidbody2D;
		public Rigidbody2D CachedRigidbody2D { get { return cachedRigidbody2D; } }

		protected readonly CachedValue<TimeComponent> cachedTime;
		public TimeComponent CachedTime { get { return cachedTime; } }

		[SerializeField]
		GravityChannel gravity = null;
		bool hasRigidbody;
		bool hasRigidbody2D;

		public GravityComponent()
		{
			cachedRigidbody = new CachedValue<Rigidbody>(GetComponent<Rigidbody>);
			cachedRigidbody2D = new CachedValue<Rigidbody2D>(GetComponent<Rigidbody2D>);
			cachedTime = new CachedValue<TimeComponent>(GetComponent<TimeComponent>);
		}

		void Awake()
		{
			hasRigidbody = CachedRigidbody != null;
			hasRigidbody2D = CachedRigidbody2D != null;
		}

		void FixedUpdate()
		{
			if (hasRigidbody)
				CachedRigidbody.velocity += gravity.Gravity * CachedTime.FixedDeltaTime;
			else if (hasRigidbody2D)
				CachedRigidbody2D.velocity += gravity.Gravity2D * CachedTime.FixedDeltaTime;
		}

		void Reset()
		{
			if (CachedRigidbody != null)
				CachedRigidbody.useGravity = false;
			else if (CachedRigidbody2D != null)
				CachedRigidbody2D.gravityScale = 0f;
		}
	}
}