using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal {
	public class StateMachineCollisionStayCaller : StateMachineCaller {

		void OnCollisionStay(Collision collision) {
			if (machine.IsActive) {
				machine.CollisionStay(collision);
			}
		}
	}
}