using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

public class Character2DMoveMoving : PState
{
	[Min]
	public float Speed = 3;
	[Min]
	public float Acceleration = 100;
	[Min]
	public float InputPower = 1;
	[Disable]
	public float CurrentSpeed;
	[Disable]
	public float CurrentAcceleration;

	new public Character2DMove Layer { get { return ((Character2DMove)base.Layer); } }

	public override void OnUpdate()
	{
		base.OnUpdate();

		if (Layer.AbsHorizontalAxis <= Layer.MoveThreshold && Layer.AbsMoveVelocity <= 0)
			SwitchState("Idle");
	}

	public override void OnFixedUpdate()
	{
		CurrentSpeed = Layer.HorizontalAxis.PowSign(InputPower) * Speed * (1F / Mathf.Max(Mathf.Sqrt(Layer.Friction), 0.0001F));
		CurrentAcceleration = Mathf.Max(Layer.Friction, 0.0001F) * Acceleration;

		if (Layer.Gravity.Angle == 90)
		{
			Layer.Rigidbody.AccelerateTowards(CurrentSpeed, CurrentAcceleration * Time.fixedDeltaTime, axes: Axes.X);
			Layer.MoveVelocity = Layer.Rigidbody.velocity.x;
		}
		else if (Layer.Gravity.Angle == 180)
		{
			Layer.Rigidbody.AccelerateTowards(-CurrentSpeed, CurrentAcceleration * Time.fixedDeltaTime, axes: Axes.Y);
			Layer.MoveVelocity = Layer.Rigidbody.velocity.y;
		}
		else if (Layer.Gravity.Angle == 270)
		{
			Layer.Rigidbody.AccelerateTowards(-CurrentSpeed, CurrentAcceleration * Time.fixedDeltaTime, axes: Axes.X);
			Layer.MoveVelocity = Layer.Rigidbody.velocity.x;
		}
		else if (Layer.Gravity.Angle == 0)
		{
			Layer.Rigidbody.AccelerateTowards(CurrentSpeed, CurrentAcceleration * Time.fixedDeltaTime, axes: Axes.Y);
			Layer.MoveVelocity = Layer.Rigidbody.velocity.y;
		}
		else
		{
			Vector3 relativeVelocity = Layer.Gravity.WorldToRelative(Layer.Rigidbody.velocity);
			relativeVelocity.x = Mathf.Lerp(relativeVelocity.x, CurrentSpeed, Time.fixedDeltaTime * CurrentAcceleration);

			Layer.MoveVelocity = relativeVelocity.x;
			Layer.Rigidbody.SetVelocity(Layer.Gravity.RelativeToWorld(relativeVelocity));
		}
	}
}
