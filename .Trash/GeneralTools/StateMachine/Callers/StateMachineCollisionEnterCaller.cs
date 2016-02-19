using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal
{
	[AddComponentMenu("")]
	public class StateMachineCollisionEnterCaller : StateMachineCaller
	{
		void OnCollisionEnter(Collision collision)
		{
			if (machine.IsActive)
				machine.CollisionEnter(collision);
		}
	}
}