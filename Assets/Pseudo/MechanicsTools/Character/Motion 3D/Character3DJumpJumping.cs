using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

public class Character3DJumpJumping : State, IInputListener
{
	[Min]
	public float MinHeight = 1;
	[Min]
	public float MaxHeight = 30;
	[Min]
	public float Duration = 0.25F;

	[Disable]
	public float Counter;
	[Disable]
	public float Increment;
	[Disable]
	public Vector2 Direction;

	new public Character3DJump Layer { get { return ((Character3DJump)base.Layer); } }

	public override void OnEnter()
	{
		base.OnEnter();

		Counter = Duration;
		Increment = (MaxHeight - MinHeight) / Duration;
		Direction = -Layer.Gravity.Direction;

		Layer.InputSystem.GetKeyboardInfo("Controller").AddListener(this);
		Layer.InputSystem.GetJoystickInfo("Controller").AddListener(this);
		Layer.Jumping = true;
		Layer.Animator.Play(Layer.JumpingHash, 1);

		if (Layer.Gravity.Angle == 90)
			Layer.Rigidbody.SetVelocity(MinHeight, Axes.Y);
		else if (Layer.Gravity.Angle == 180)
			Layer.Rigidbody.SetVelocity(MinHeight, Axes.X);
		else if (Layer.Gravity.Angle == 270)
			Layer.Rigidbody.SetVelocity(-MinHeight, Axes.Y);
		else if (Layer.Gravity.Angle == 0)
			Layer.Rigidbody.SetVelocity(-MinHeight, Axes.X);
		else
		{
			Vector2 relativeVelocity = Layer.Gravity.WorldToRelative(Layer.Rigidbody.velocity);
			relativeVelocity.y = MinHeight;

			Layer.Rigidbody.SetVelocity(Layer.Gravity.RelativeToWorld(relativeVelocity));
		}
	}

	public override void OnExit()
	{
		base.OnExit();

		Layer.InputSystem.GetKeyboardInfo("Controller").RemoveListener(this);
		Layer.InputSystem.GetJoystickInfo("Controller").RemoveListener(this);
		Layer.Jumping = false;
	}

	public override void OnFixedUpdate()
	{
		base.OnFixedUpdate();

		Counter -= Time.fixedDeltaTime;

		if (Counter > 0)
			Layer.Rigidbody.Accelerate(Direction * Increment * (Counter / Duration), Axes.XY);
		else
			SwitchState("Falling");
	}

	public void OnButtonInput(ButtonInput input)
	{
		switch (input.InputName)
		{
			case "Jump":
				if (input.State != ButtonStates.Pressed)
					SwitchState("Falling");
				break;
		}
	}

	public void OnAxisInput(AxisInput input)
	{

	}
}
