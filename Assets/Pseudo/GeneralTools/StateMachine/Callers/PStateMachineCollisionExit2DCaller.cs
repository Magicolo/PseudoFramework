using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal {
	public class PStateMachineCollisionExit2DCaller : StateMachineCaller {

		void OnCollisionExit2D(Collision2D collision) {
			if (machine.IsActive) {
				machine.CollisionExit2D(collision);
			}
		}
	}
}