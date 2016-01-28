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
	public class OnCollisionEnter2DRelay : PhysicsEventRelayBase
	{
		void OnCollisionEnter2D(Collision2D collision)
		{
			Relay.EnqueueEvent(PhysicsEvents.OnCollisionEnter2D, collision);
		}
	}
}