using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Internal;

namespace Pseudo
{
	[System.Serializable]
	public class PJoystickAxis : PAxisBase
	{
		[SerializeField]
		Joysticks _joystick;
		public Joysticks Joystick
		{
			get { return _joystick; }
			set
			{
				_joystick = value;

				axis = PInput.JoystickInputToAxis(_joystick, _axisInput);
				lastValue = 0;
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

				axis = PInput.JoystickInputToAxis(_joystick, _axisInput);
				lastValue = 0;
			}
		}

		public override string Axis
		{
			get { return axis; }
			set
			{
				axis = value;

				_joystick = PInput.AxisToJoystick(axis);
				_axisInput = PInput.AxisToJoystickAxis(axis);
				lastValue = 0;
			}
		}

		public PJoystickAxis(string name, Joysticks joystick, JoystickAxes axis) : base(name, PInput.JoystickInputToAxis(joystick, axis))
		{
			_joystick = joystick;
			_axisInput = axis;
		}

		public PJoystickAxis(string name, string axisName) : base(name, axisName)
		{
			_joystick = PInput.AxisToJoystick(axisName);
			_axisInput = PInput.AxisToJoystickAxis(axisName);
		}
	}
}