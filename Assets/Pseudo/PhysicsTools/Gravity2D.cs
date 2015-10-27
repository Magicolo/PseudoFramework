using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Internal.Physics;

namespace Pseudo
{
	[RequireComponent(typeof(Rigidbody2D))]
	[AddComponentMenu("Pseudo/Physics/Gravity2D")]
	public class Gravity2D : GravityBase
	{
		public CachedValue<Rigidbody2D> CachedRigidbody;

		public Gravity2D()
		{
			CachedRigidbody = new CachedValue<Rigidbody2D>(CachedGameObject.FindComponent<Rigidbody2D>);
		}

		void FixedUpdate()
		{
			CachedRigidbody.Value.AddForce(Force * CachedRigidbody.Value.mass);
		}
	}
}

