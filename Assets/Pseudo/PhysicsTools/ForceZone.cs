using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Internal.PhysicsTools;

namespace Pseudo
{
	[AddComponentMenu("Pseudo/Physics/Force Zone")]
	public class ForceZone : PhysicsZone
	{
		public Vector2 Force;
		[Range(0, 1)]
		public float Damping;
		[Range(0, 1)]
		public float DistanceScaling;

		bool _colliderCached;
		Collider _collider;
		public Collider Collider
		{
			get
			{
				_collider = _colliderCached ? _collider : this.FindComponent<Collider>();
				_colliderCached = true;
				return _collider;
			}
		}

		void FixedUpdate()
		{
			foreach (KeyValuePair<Rigidbody, int> pair in RigidbodyCountDict)
			{
				Vector2 adjustedForce = Force;
				float adjustedDamping = Damping;

				if (DistanceScaling > 0)
				{
					Bounds zoneBounds = Collider.bounds;
					Vector3 bodyPosition = pair.Key.transform.position;
					float xAttenuation = Mathf.Clamp01(Mathf.Abs(zoneBounds.center.x - bodyPosition.x) / zoneBounds.extents.x) * DistanceScaling;
					float yAttenuation = Mathf.Clamp01(Mathf.Abs(zoneBounds.center.y - bodyPosition.y) / zoneBounds.extents.y) * DistanceScaling;
					float attenuation = 1 - (xAttenuation + yAttenuation) / 2;
					attenuation *= attenuation;

					adjustedForce *= attenuation;
					adjustedDamping *= attenuation;
				}

				pair.Key.AddForce(Force);

				if (adjustedDamping > 0)
					pair.Key.SetVelocity(pair.Key.velocity * (1 - adjustedDamping));
			}
		}
	}
}