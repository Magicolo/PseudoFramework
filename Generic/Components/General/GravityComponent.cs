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

		[SerializeField]
		GravityChannel gravity = new GravityChannel();
		public TimeComponent Time;

		Rigidbody body;
		Rigidbody2D body2D;
		bool hasRigidbody;
		bool hasRigidbody2D;

		void Awake()
		{
			body = GetComponent<Rigidbody>();
			body2D = GetComponent<Rigidbody2D>();
			hasRigidbody = body != null;
			hasRigidbody2D = body2D != null;
		}

		void FixedUpdate()
		{
			if (hasRigidbody)
				body.velocity += gravity.Gravity * Time.FixedDeltaTime;
			else if (hasRigidbody2D)
				body2D.velocity += gravity.Gravity2D * Time.FixedDeltaTime;
		}

		void OnCreate()
		{
			gravity.Reset();
		}

		public static implicit operator GravityChannel(GravityComponent gravity)
		{
			return gravity.gravity;
		}
	}
}