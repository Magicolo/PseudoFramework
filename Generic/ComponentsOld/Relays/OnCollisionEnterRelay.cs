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
	public class OnCollisionEnterRelay : PhysicsEventRelayBase
	{
		void OnCollisionEnter(Collision collision)
		{
			Relay.EnqueueEvent(PhysicsEvents.OnCollisionEnter, collision);
		}
	}
}