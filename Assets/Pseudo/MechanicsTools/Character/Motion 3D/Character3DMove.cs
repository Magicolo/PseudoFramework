using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

public class Character3DMove : PStateLayer, IInputListener
{
	[Min]
	public float MoveThreshold;
	[Min]
	public float InputPower = 1;
	[Disable]
	public float Velocity;

	public Gravity Gravity { get { return Layer.Gravity; } }
	public float HorizontalAxis { get { return Layer.HorizontalAxis; } set { Layer.HorizontalAxis = value; } }
	public float AbsHorizontalAxis { get { return Layer.AbsHorizontalAxis; } }
	public float MoveVelocity { get { return Layer.MoveVelocity; } set { Layer.MoveVelocity = value; } }
	public float AbsMoveVelocity { get { return Layer.AbsMoveVelocity; } }
	public float Friction { get { return Layer.Friction; } }
	public Animator Animator { get { return Layer.Animator; } }
	public Rigidbody Rigidbody { get { return Layer.Rigidbody; } }
	public InputSystem InputSystem { get { return Layer.InputSystem; } }
	new public Character3DMotion Layer { get { return ((Character3DMotion)base.Layer); } }

	public override void OnEnter()
	{
		base.OnEnter();

		InputSystem.GetKeyboardInfo("Controller").AddListener(this);
		InputSystem.GetJoystickInfo("Controller").AddListener(this);
	}

	public override void OnExit()
	{
		base.OnExit();

		InputSystem.GetKeyboardInfo("Controller").RemoveListener(this);
		InputSystem.GetJoystickInfo("Controller").RemoveListener(this);
	}

	public void OnButtonInput(ButtonInput input)
	{

	}

	public void OnAxisInput(AxisInput input)
	{
		switch (input.InputName)
		{
			case "MotionX":
				HorizontalAxis = input.Value.PowSign(InputPower);
				break;
		}
	}
}
