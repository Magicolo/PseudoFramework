using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Internal.Physics;

namespace Pseudo
{
	[RequireComponent(typeof(Rigidbody))]
	[AddComponentMenu("Pseudo/Physics/Gravity")]
	public class Gravity : GravityBase
	{
		readonly CachedValue<Rigidbody> cachedRigidbody;
		public Rigidbody Rigidbody { get { return cachedRigidbody; } }

		public Gravity()
		{
			cachedRigidbody = new CachedValue<Rigidbody>(GetComponent<Rigidbody>);
		}

		void FixedUpdate()
		{
			cachedRigidbody.Value.velocity += gravity * TimeManager.GetFixedDeltaTime(TimeChannel);
		}

		void Reset()
		{
			cachedRigidbody.Value.useGravity = false;
		}
	}
}

