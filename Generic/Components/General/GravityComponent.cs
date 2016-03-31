using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Physics.Internal;
using Pseudo.EntityFramework;
using Pseudo.Pooling;
using Pseudo.Physics;

namespace Pseudo
{
	[AddComponentMenu("Pseudo/Physics/Gravity")]
	[RequireComponent(typeof(TimeComponent))]
	public class GravityComponent : ComponentBehaviourBase, IGravityChannel
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

		readonly Lazy<Rigidbody> cachedRigidbody;
		public Rigidbody Rigidbody { get { return cachedRigidbody; } }

		readonly Lazy<Rigidbody2D> cachedRigidbody2D;
		public Rigidbody2D Rigidbody2D { get { return cachedRigidbody2D; } }

		[SerializeField, InitializeContent]
		GravityChannel gravity = new GravityChannel();
		public TimeComponent Time;

		bool hasRigidbody;
		bool hasRigidbody2D;

		public GravityComponent()
		{
			cachedRigidbody = new Lazy<Rigidbody>(GetComponent<Rigidbody>);
			cachedRigidbody2D = new Lazy<Rigidbody2D>(GetComponent<Rigidbody2D>);
		}

		public override void OnAdded()
		{
			base.OnAdded();

			gravity.Reset();
		}

		void Awake()
		{
			hasRigidbody = Rigidbody != null;
			hasRigidbody2D = Rigidbody2D != null;
		}

		void FixedUpdate()
		{
			if (hasRigidbody)
				Rigidbody.velocity += gravity.Gravity * Time.FixedDeltaTime;
			else if (hasRigidbody2D)
				Rigidbody2D.velocity += gravity.Gravity2D * Time.FixedDeltaTime;
		}

		public static implicit operator GravityChannel(GravityComponent gravity)
		{
			return gravity.gravity;
		}
	}
}