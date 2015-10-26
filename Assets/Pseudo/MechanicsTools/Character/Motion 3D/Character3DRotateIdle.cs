using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

public class Character3DRotateIdle : State
{
	new public Character3DRotate Layer { get { return ((Character3DRotate)base.Layer); } }

	public override void OnUpdate()
	{
		base.OnUpdate();

		if (Mathf.Abs(Layer.CurrentFacingDirection - Layer.TargetFacingAngle) > 0.0001F || Mathf.Abs(Layer.CurrentAngle - Layer.TargetAngle) > 0.0001F)
			SwitchState("Rotating");
	}
}
