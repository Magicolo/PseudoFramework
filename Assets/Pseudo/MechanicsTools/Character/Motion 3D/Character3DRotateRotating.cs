using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

public class Character3DRotateRotating : State
{
	[Min]
	public float Speed = 20;

	new public Character3DRotate Layer { get { return ((Character3DRotate)base.Layer); } }

	public override void OnUpdate()
	{
		base.OnUpdate();

		if (Mathf.Abs(Layer.CurrentFacingDirection - Layer.TargetFacingAngle) <= 0.0001F && Mathf.Abs(Layer.CurrentAngle - Layer.TargetAngle) <= 0.0001F)
		{
			SwitchState("Idle");
			return;
		}

		Layer.CurrentFacingDirection = Mathf.Lerp(Layer.modelTransform.localEulerAngles.y, Layer.TargetFacingAngle, Speed * Time.deltaTime);
		Layer.modelTransform.RotateLocalTowards(Layer.CurrentFacingDirection, Speed * Time.deltaTime, axes: Axes.Y);
	}

	public override void OnFixedUpdate()
	{
		Layer.CurrentAngle = Mathf.LerpAngle(Transform.localEulerAngles.z, Layer.TargetAngle, Speed * Time.fixedDeltaTime);
		Layer.Rigidbody.RotateTowards(Layer.CurrentAngle, Speed, Axes.Z);
	}
}
