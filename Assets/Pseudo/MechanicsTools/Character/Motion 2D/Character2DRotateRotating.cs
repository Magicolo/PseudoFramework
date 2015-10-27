using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

public class Character2DRotateRotating : PState
{
	[Min]
	public float Speed = 20;

	new public Character2DRotate Layer { get { return ((Character2DRotate)base.Layer); } }

	public override void OnUpdate()
	{
		base.OnUpdate();

		if (Mathf.Abs(Layer.CurrentFacingDirection - Layer.TargetFacingDirection) <= 0.0001F && Mathf.Abs(Layer.CurrentAngle - Layer.TargetAngle) <= 0.0001F)
		{
			SwitchState("Idle");
			return;
		}

		Layer.CurrentFacingDirection = Mathf.Lerp(Layer.SpriteTransform.localScale.x, Layer.TargetFacingDirection, Speed * Time.deltaTime);
		Layer.SpriteTransform.ScaleLocalTowards(Layer.CurrentFacingDirection, Speed * Time.deltaTime, axes: Axes.X);
	}

	public override void OnFixedUpdate()
	{
		Layer.CurrentAngle = Mathf.LerpAngle(CachedTransform.localEulerAngles.z, Layer.TargetAngle, Speed * Time.fixedDeltaTime);
		Layer.Rigidbody.RotateTowards(Layer.CurrentAngle, Speed * Time.fixedDeltaTime);
	}
}
