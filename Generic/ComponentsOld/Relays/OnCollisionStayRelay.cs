using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal;

namespace Pseudo.Interal
{
	[AddComponentMenu("")]
	public class OnCollisionStayRelay : PhysicsEventRelayBase
	{
		void OnCollisionStay(Collision collision)
		{
			Relay.EnqueueEvent(PhysicsEvents.OnCollisionStay, collision);
		}
	}
}