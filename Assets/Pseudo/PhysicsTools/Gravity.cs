using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Internal.PhysicsTools;

namespace Pseudo
{
	[RequireComponent(typeof(Rigidbody))]
	[AddComponentMenu("Pseudo/Physics/Gravity")]
	public class Gravity : GravityBase
	{
		bool _rigidbodyCached;
		Rigidbody _rigidbody;
		public Rigidbody Rigidbody
		{
			get
			{
				_rigidbody = _rigidbodyCached ? _rigidbody : this.FindComponent<Rigidbody>();
				_rigidbodyCached = true;
				return _rigidbody;
			}
		}

		void FixedUpdate()
		{
			Rigidbody.AddForce(Force * Rigidbody.mass);
		}
	}
}

