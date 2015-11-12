using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal.Physics;

namespace Pseudo
{
	[AddComponentMenu("Pseudo/Physics/Gravity2D")]
	[RequireComponent(typeof(Rigidbody2D), typeof(TimeComponent))]
	public class Gravity2DComponent : GravityComponentBase
	{
		protected readonly CachedValue<Rigidbody2D> cachedRigidbody2D;
		public Rigidbody2D CachedRigidbody2D { get { return cachedRigidbody2D; } }

		protected readonly CachedValue<TimeComponent> cachedTimeComponent;
		public TimeComponent CachedTimeComponent { get { return cachedTimeComponent; } }

		public Gravity2DComponent()
		{
			cachedRigidbody2D = new CachedValue<Rigidbody2D>(GetComponent<Rigidbody2D>);
			cachedTimeComponent = new CachedValue<TimeComponent>(GetComponent<TimeComponent>);
		}

		protected virtual void FixedUpdate()
		{
			cachedRigidbody2D.Value.velocity += gravity.ToVector2() * cachedTimeComponent.Value.FixedDeltaTime;
		}

		protected virtual void Reset()
		{
			cachedRigidbody2D.Value.gravityScale = 0f;
		}

		protected override Vector3 GetGravity()
		{
			return GravityManager.GetGravity(channel);
		}
	}
}