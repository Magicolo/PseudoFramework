using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

public class CharacterStatus : PStateLayer {
	
	public void Die() {
		SwitchState<CharacterDie>();
	}
}
