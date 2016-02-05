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
	public class OnTriggerEnterRelay : PhysicsEventRelayBase
	{
		void OnTriggerEnter(Collider collision)
		{
			Relay.EnqueueEvent(PhysicsEvents.OnTriggerEnter, collision);
		}
	}
}