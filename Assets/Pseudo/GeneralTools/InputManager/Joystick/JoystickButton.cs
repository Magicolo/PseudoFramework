using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Internal;

namespace Pseudo
{
	[System.Serializable]
	public class JoystickButton : ButtonBase
	{
		[SerializeField]
		Joysticks joystick;
		public Joysticks Joystick
		{
			get { return joystick; }
			set
			{
				joystick = value;
				key = InputManager.JoystickInputToKey(joystick, button);
			}
		}

		[SerializeField, PropertyField]
		JoystickButtons button;
		public JoystickButtons Button
		{
			get { return button; }
			set
			{
				button = value;
				key = InputManager.JoystickInputToKey(joystick, button);
			}
		}

		public override KeyCode Key
		{
			get { return key; }
			set
			{
				key = value;
				joystick = InputManager.KeyToJoystick(key);
				button = InputManager.KeyToJoystickButton(key);
			}
		}

		public JoystickButton(string name, Joysticks joystick, JoystickButtons button) : base(name, InputManager.JoystickInputToKey(joystick, button))
		{
			this.joystick = joystick;
			this.button = button;
		}

		public JoystickButton(string name, KeyCode key) : base(name, key)
		{
			joystick = InputManager.KeyToJoystick(key);
			button = InputManager.KeyToJoystickButton(key);
		}
	}
}