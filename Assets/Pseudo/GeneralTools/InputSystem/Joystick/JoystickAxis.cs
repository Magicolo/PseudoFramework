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
		Joysticks _joystick;
		public Joysticks Joystick
		{
			get { return _joystick; }
			set
			{
				_joystick = value;

				_axis = InputSystem.JoystickInputToAxis(_joystick, _axisInput);
				_lastValue = 0;
			}
		}

		[SerializeField, PropertyField]
		JoystickAxes _axisInput;
		public JoystickAxes AxisInput
		{
			get { return _axisInput; }
			set
			{
				_axisInput = value;

				_axis = InputSystem.JoystickInputToAxis(_joystick, _axisInput);
				_lastValue = 0;
			}
		}

		public override string Axis
		{
			get { return _axis; }
			set
			{
				_axis = value;

				_joystick = InputSystem.AxisToJoystick(_axis);
				_axisInput = InputSystem.AxisToJoystickAxis(_axis);
				_lastValue = 0;
			}
		}

		public JoystickAxis(string name, Joysticks joystick, JoystickAxes axis) : base(name, InputSystem.JoystickInputToAxis(joystick, axis))
		{
			_joystick = joystick;
			_axisInput = axis;
		}

		public JoystickAxis(string name, string axisName) : base(name, axisName)
		{
			_joystick = InputSystem.AxisToJoystick(axisName);
			_axisInput = InputSystem.AxisToJoystickAxis(axisName);
		}
	}
}