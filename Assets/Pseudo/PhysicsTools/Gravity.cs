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
		public CachedValue<Rigidbody> CachedRigidbody;

		public Gravity()
		{
			CachedRigidbody = new CachedValue<Rigidbody>(CachedGameObject.FindComponent<Rigidbody>);
		}

		void FixedUpdate()
		{
			CachedRigidbody.Value.AddForce(Force * CachedRigidbody.Value.mass);
		}
	}
}

