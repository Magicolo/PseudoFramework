using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

public class Character3DJumpIdle : State, IInputListener
{
	new public Character3DJump Layer { get { return ((Character3DJump)Layer); } }

	public override void OnEnter()
	{
		base.OnEnter();

		Layer.InputSystem.GetKeyboardInfo("Controller").AddListener(this);
		Layer.InputSystem.GetJoystickInfo("Controller").AddListener(this);
	}

	public override void OnExit()
	{
		base.OnExit();

		Layer.InputSystem.GetKeyboardInfo("Controller").RemoveListener(this);
		Layer.InputSystem.GetJoystickInfo("Controller").RemoveListener(this);
	}

	public override void OnUpdate()
	{
		base.OnUpdate();

		if (!Layer.Grounded)
		{
			SwitchState("Falling");
			return;
		}
	}

	public void OnButtonInput(ButtonInput input)
	{
		switch (input.InputName)
		{
			case "Jump":
				if (input.State == ButtonStates.Down)
					SwitchState("Jumping");
				break;
		}
	}

	public void OnAxisInput(AxisInput input)
	{

	}
}
