using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal
{
	[System.Serializable]
	public class JoystickInput
	{
		readonly string joystickName;
		public string JoystickName
		{
			get
			{
				return joystickName;
			}
		}

		readonly Joysticks joystick;
		public Joysticks Joystick
		{
			get
			{
				return joystick;
			}
		}

		readonly string inputName;
		public string InputName
		{
			get
			{
				return inputName;
			}
		}

		public JoystickInput(string joystickName, Joysticks joystick, string inputName)
		{
			this.joystickName = joystickName;
			this.joystick = joystick;
			this.inputName = inputName;
		}
	}
}