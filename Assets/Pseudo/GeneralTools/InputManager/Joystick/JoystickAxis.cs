using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Internal;

namespace Pseudo
{
	[System.Serializable]
	public class JoystickAxis : AxisBase
	{
		[SerializeField]
		Joysticks joystick;
		public Joysticks Joystick
		{
			get { return joystick; }
			set
			{
				joystick = value;

				axis = InputManager.JoystickInputToAxis(joystick, axisInput);
				lastValue = 0;
			}
		}

		[SerializeField, PropertyField]
		JoystickAxes axisInput;
		public JoystickAxes AxisInput
		{
			get { return axisInput; }
			set
			{
				axisInput = value;

				axis = InputManager.JoystickInputToAxis(joystick, axisInput);
				lastValue = 0;
			}
		}

		public override string Axis
		{
			get { return axis; }
			set
			{
				axis = value;

				joystick = InputManager.AxisToJoystick(axis);
				axisInput = InputManager.AxisToJoystickAxis(axis);
				lastValue = 0;
			}
		}

		public JoystickAxis(string name, Joysticks joystick, JoystickAxes axis) : base(name, InputManager.JoystickInputToAxis(joystick, axis))
		{
			this.joystick = joystick;
			axisInput = axis;
		}

		public JoystickAxis(string name, string axisName) : base(name, axisName)
		{
			joystick = InputManager.AxisToJoystick(axisName);
			axisInput = InputManager.AxisToJoystickAxis(axisName);
		}
	}
}