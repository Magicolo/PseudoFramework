using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Internal.PhysicsTools;

namespace Pseudo
{
	[AddComponentMenu("Pseudo/Physics/Gravity Zone")]
	public class GravityZone : PhysicsZone
	{

		public Vector2 gravity;

		Dictionary<Gravity, Vector2> gravityDict;
		public Dictionary<Gravity, Vector2> GravityDict
		{
			get
			{
				if (gravityDict == null)
				{
					gravityDict = new Dictionary<Gravity, Vector2>();
				}

				return gravityDict;
			}
		}

		public override void OnRigidbodyEnter(Rigidbody attachedRigidbody)
		{
			base.OnRigidbodyEnter(attachedRigidbody);

			Gravity attachedGravity = attachedRigidbody.FindComponent<Gravity>();

			if (attachedGravity != null)
			{
				GravityDict[attachedGravity] = attachedGravity.Force;
				attachedGravity.Force = gravity;
			}
		}

		public override void OnRigidbodyExit(Rigidbody attachedRigidbody)
		{
			base.OnRigidbodyExit(attachedRigidbody);

			Gravity attachedGravity = attachedRigidbody.FindComponent<Gravity>();

			if (attachedGravity != null && GravityDict.ContainsKey(attachedGravity))
			{
				attachedGravity.Force = GravityDict[attachedGravity];
				GravityDict.Remove(attachedGravity);
			}
		}
	}
}