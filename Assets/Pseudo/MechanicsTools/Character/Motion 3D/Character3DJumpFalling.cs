using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

public class Character3DJumpFalling : State
{
	new public Character3DJump Layer { get { return ((Character3DJump)base.Layer); } }

	public override void OnUpdate()
	{
		base.OnUpdate();

		if (Layer.Grounded)
		{
			SwitchState("Idle");
			return;
		}
	}
}
