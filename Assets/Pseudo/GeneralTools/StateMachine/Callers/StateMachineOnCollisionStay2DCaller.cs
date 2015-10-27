using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal {
	public class StateMachineCollisionStay2DCaller : StateMachineCaller {

		void OnCollisionStay2D(Collision2D collision) {
			if (machine.IsActive) {
				machine.CollisionStay2D(collision);
			}
		}
	}
}