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
	public class GravityComponent : ComponentBehaviour, IGravityChannel
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

		protected readonly Lazy<Rigidbody> cachedRigidbody;
		public Rigidbody CachedRigidbody { get { return cachedRigidbody; } }

		protected readonly Lazy<Rigidbody2D> cachedRigidbody2D;
		public Rigidbody2D CachedRigidbody2D { get { return cachedRigidbody2D; } }

		protected readonly Lazy<TimeComponent> cachedTime;
		public TimeComponent CachedTime { get { return cachedTime; } }

		[SerializeField, InitializeContent]
		GravityChannel gravity = new GravityChannel();
		bool hasRigidbody;
		bool hasRigidbody2D;

		public GravityComponent()
		{
			cachedRigidbody = new Lazy<Rigidbody>(GetComponent<Rigidbody>);
			cachedRigidbody2D = new Lazy<Rigidbody2D>(GetComponent<Rigidbody2D>);
			cachedTime = new Lazy<TimeComponent>(GetComponent<TimeComponent>);
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