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
	public class OnCollisionExitRelay : PhysicsEventRelayBase
	{
		void OnCollisionExit(Collision collision)
		{
			Relay.EnqueueEvent(PhysicsEvents.OnCollisionExit, collision);
		}
	}
}