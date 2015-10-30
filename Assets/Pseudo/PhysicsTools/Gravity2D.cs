using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Internal.Physics;
using System;

namespace Pseudo
{
	[RequireComponent(typeof(Rigidbody2D))]
	[AddComponentMenu("Pseudo/Physics/Gravity2D")]
	public class Gravity2D : GravityBase
	{
		readonly CachedValue<Rigidbody2D> cachedRigidbody;
		public Rigidbody2D Rigidbody { get { return cachedRigidbody; } }

		public Gravity2D()
		{
			cachedRigidbody = new CachedValue<Rigidbody2D>(GetComponent<Rigidbody2D>);
		}

		void FixedUpdate()
		{
			cachedRigidbody.Value.velocity += gravity.ToVector2() * TimeManager.GetFixedDeltaTime(TimeChannel);
		}

		void Reset()
		{
			cachedRigidbody.Value.gravityScale = 0f;
		}
	}
}

