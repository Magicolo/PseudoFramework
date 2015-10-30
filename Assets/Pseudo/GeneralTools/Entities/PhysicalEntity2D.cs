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
		public LocalGravityChannel GravitySettings;

		protected readonly CachedValue<Rigidbody2D> cachedRigidbody;
		public Rigidbody2D CachedRigidbody { get { return cachedRigidbody; } }

		public PhysicalEntity2D()
		{
			cachedRigidbody = new CachedValue<Rigidbody2D>(GetComponent<Rigidbody2D>);
		}

		protected virtual void UpdateGravity()
		{
			cachedRigidbody.Value.velocity += GravitySettings.Gravity.ToVector2() * TimeSettings.FixedDeltaTime;
		}

		protected virtual void Reset()
		{
			cachedRigidbody.Value.gravityScale = 0f;
		}
	}
}