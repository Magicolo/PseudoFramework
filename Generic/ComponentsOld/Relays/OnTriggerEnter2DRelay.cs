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
	public class OnTriggerEnter2DRelay : PhysicsEventRelayBase
	{
		void OnTriggerEnter2D(Collider2D collision)
		{
			Relay.EnqueueEvent(PhysicsEvents.OnTriggerEnter2D, collision);
		}
	}
}