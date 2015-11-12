using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal.Physics;

namespace Pseudo
{
	[AddComponentMenu("Pseudo/Physics/Gravity")]
	[RequireComponent(typeof(Rigidbody), typeof(TimeComponent))]
	public class GravityComponent : GravityComponentBase
	{
		protected readonly CachedValue<Rigidbody> cachedRigidbody;
		public Rigidbody CachedRigidbody { get { return cachedRigidbody; } }

		protected readonly CachedValue<TimeComponent> cachedTimeComponent;
		public TimeComponent CachedTimeComponent { get { return cachedTimeComponent; } }

		public GravityComponent()
		{
			cachedRigidbody = new CachedValue<Rigidbody>(GetComponent<Rigidbody>);
			cachedTimeComponent = new CachedValue<TimeComponent>(GetComponent<TimeComponent>);
		}

		protected virtual void FixedUpdate()
		{
			cachedRigidbody.Value.velocity += Gravity * cachedTimeComponent.Value.FixedDeltaTime;
		}

		protected virtual void Reset()
		{
			cachedRigidbody.Value.useGravity = false;
		}

		protected override Vector3 GetGravity()
		{
			return GravityManager.GetGravity(channel);
		}
	}
}