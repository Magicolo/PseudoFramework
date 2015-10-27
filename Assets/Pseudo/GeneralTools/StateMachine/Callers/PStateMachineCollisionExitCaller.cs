using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal {
	public class PStateMachineCollisionExitCaller : StateMachineCaller {

		void OnCollisionExit(Collision collision) {
			if (machine.IsActive) {
				machine.CollisionExit(collision);
			}
		}
	}
}