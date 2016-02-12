using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal
{
	[AddComponentMenu("")]
	public class StateMachineTriggerExitCaller : StateMachineCaller
	{
		void OnTriggerExit(Collider collision)
		{
			if (machine.IsActive)
				machine.TriggerExit(collision);
		}
	}
}