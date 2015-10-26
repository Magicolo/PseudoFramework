using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

public class Character2DRotateIdle : State
{
	new public Character2DRotate Layer { get { return ((Character2DRotate)base.Layer); } }

	public override void OnUpdate()
	{
		base.OnUpdate();

		if (Mathf.Abs(Layer.CurrentFacingDirection - Layer.TargetFacingDirection) > 0.0001F || Mathf.Abs(Layer.CurrentAngle - Layer.TargetAngle) > 0.0001F)
			SwitchState("Rotating");
	}
}
