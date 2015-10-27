using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal {
	public class StateMachineTriggerStay2DCaller : StateMachineCaller {

		void OnTriggerStay2D(Collider2D collision) {
			if (machine.IsActive) {
				machine.TriggerStay2D(collision);
			}
		}
	}
}