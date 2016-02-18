using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal
{
	[AddComponentMenu("")]
	public class StateMachineCollisionExitCaller : StateMachineCaller
	{
		void OnCollisionExit(Collision collision)
		{
			if (machine.IsActive)
				machine.CollisionExit(collision);
		}
	}
}