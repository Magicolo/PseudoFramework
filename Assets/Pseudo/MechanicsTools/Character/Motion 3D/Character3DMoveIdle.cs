using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

public class Character3DMoveIdle : PState
{
	new public Character3DMove Layer { get { return ((Character3DMove)base.Layer); } }

	public override void OnUpdate()
	{
		base.OnUpdate();

		if (Layer.AbsHorizontalAxis > Layer.MoveThreshold || Layer.AbsMoveVelocity > Layer.MoveThreshold)
		{
			SwitchState("Moving");
			return;
		}

		if (Layer.Gravity.Angle == 90)
			Layer.MoveVelocity = Layer.Rigidbody.velocity.x;
		else if (Layer.Gravity.Angle == 180)
			Layer.MoveVelocity = Layer.Rigidbody.velocity.y;
		else if (Layer.Gravity.Angle == 270)
			Layer.MoveVelocity = Layer.Rigidbody.velocity.x;
		else if (Layer.Gravity.Angle == 0)
			Layer.MoveVelocity = Layer.Rigidbody.velocity.y;
		else
			Layer.MoveVelocity = Layer.Gravity.WorldToRelative(Layer.Rigidbody.velocity).x;
	}
}
