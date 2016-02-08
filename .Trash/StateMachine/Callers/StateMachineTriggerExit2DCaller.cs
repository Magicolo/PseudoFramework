using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal
{
	[AddComponentMenu("")]
	public class StateMachineTriggerExit2DCaller : StateMachineCaller
	{
		void OnTriggerExit2D(Collider2D collision)
		{
			if (machine.IsActive)
				machine.TriggerExit2D(collision);
		}
	}
}