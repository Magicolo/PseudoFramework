using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal
{
	[AddComponentMenu("")]
	public class StateMachineCollisionEnter2DCaller : StateMachineCaller
	{
		void OnCollisionEnter2D(Collision2D collision)
		{
			if (machine.IsActive)
				machine.CollisionEnter2D(collision);
		}
	}
}