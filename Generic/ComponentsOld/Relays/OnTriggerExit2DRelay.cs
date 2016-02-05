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
	public class OnTriggerExit2DRelay : PhysicsEventRelayBase
	{
		void OnTriggerExit2D(Collider2D collision)
		{
			Relay.EnqueueEvent(PhysicsEvents.OnTriggerExit2D, collision);
		}
	}
}