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
	public class OnCollisionStay2DRelay : PhysicsEventRelayBase
	{
		void OnCollisionStay2D(Collision2D collision)
		{
			Relay.EnqueueEvent(PhysicsEvents.OnCollisionStay2D, collision);
		}
	}
}