using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal
{
	[AddComponentMenu("")]
	public class StateMachineTriggerEnter2DCaller : StateMachineCaller
	{
		void OnTriggerEnter2D(Collider2D collision)
		{
			if (machine.IsActive)
				machine.TriggerEnter2D(collision);
		}
	}
}