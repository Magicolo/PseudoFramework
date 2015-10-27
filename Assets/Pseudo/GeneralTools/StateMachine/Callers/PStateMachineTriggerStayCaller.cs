using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal {
	public class PStateMachineTriggerStayCaller : StateMachineCaller {

		void OnTriggerStay(Collider collision) {
			if (machine.IsActive) {
				machine.TriggerStay(collision);
			}
		}
	}
}