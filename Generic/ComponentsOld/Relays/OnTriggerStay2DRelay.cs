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
	public class OnTriggerStay2DRelay : PhysicsEventRelayBase
	{
		void OnTriggerStay2D(Collider2D collision)
		{
			Relay.EnqueueEvent(PhysicsEvents.OnTriggerStay2D, collision);
		}
	}
}