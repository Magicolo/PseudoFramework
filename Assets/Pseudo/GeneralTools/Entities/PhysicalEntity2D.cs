using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo
{
	[RequireComponent(typeof(Rigidbody2D))]
	public class PhysicalEntity2D : EntityBase
	{
		[Serializable]
		public class GravityChannel2D
		{
			[SerializeField, PropertyField]
			GravityManager.GravityChannels channel;
			[SerializeField, PropertyField]
			Vector2 gravityScale = new Vector3(1f, 1f, 1f);
			[SerializeField, PropertyField(typeof(RangeAttribute), 0f, 360f)]
			float angle;

			Vector2 gravity;
			Vector2 lastGravity;
			bool hasChanged = true;

			public GravityManager.GravityChannels Channel
			{
				get { return channel; }
				set
				{
					channel = value;
					hasChanged = true;
				}
			}
			public Vector2 GravityScale
			{
				get { return gravityScale; }
				set
				{
					gravityScale = value;
					hasChanged = true;
				}
			}
			public float Angle
			{
				get { return angle; }
				set
				{
					angle = value;
					hasChanged = true;
				}
			}
			public Vector2 Gravity
			{
				get
				{
					UpdateGravityForce();
					return gravity;
				}
			}

			void UpdateGravityForce()
			{
				Vector2 currentGravity = GravityManager.GetGravity(channel);

				if (!hasChanged && lastGravity == currentGravity)
					return;

				lastGravity = currentGravity;
				hasChanged = false;
				gravity = lastGravity.Mult(gravityScale).Rotate(angle);
			}
		}

		public GravityChannel2D GravitySettings;

		protected readonly CachedValue<Rigidbody2D> cachedRigidbody;
		public Rigidbody2D CachedRigidbody { get { return cachedRigidbody; } }

		public PhysicalEntity2D()
		{
			cachedRigidbody = new CachedValue<Rigidbody2D>(GetComponent<Rigidbody2D>);
		}

		protected virtual void FixedUpdate()
		{
			cachedRigidbody.Value.velocity += GravitySettings.Gravity * TimeSettings.FixedDeltaTime;
		}

		protected virtual void Reset()
		{
			cachedRigidbody.Value.gravityScale = 0f;
		}
	}
}