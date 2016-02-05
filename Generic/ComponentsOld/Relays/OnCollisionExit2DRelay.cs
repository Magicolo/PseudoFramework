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
	public class OnCollisionExit2DRelay : PhysicsEventRelayBase
	{
		void OnCollisionExit2D(Collision2D collision)
		{
			Relay.EnqueueEvent(PhysicsEvents.OnCollisionExit2D, collision);
		}
	}
}