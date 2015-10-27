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
		Joysticks _joystick;
		public Joysticks Joystick
		{
			get { return _joystick; }
			set
			{
				_joystick = value;
				key = PInput.JoystickInputToKey(_joystick, _button);
			}
		}

		[SerializeField, PropertyField]
		JoystickButtons _button;
		public JoystickButtons Button
		{
			get { return _button; }
			set
			{
				_button = value;
				key = PInput.JoystickInputToKey(_joystick, _button);
			}
		}

		public override KeyCode Key
		{
			get { return key; }
			set
			{
				key = value;
				_joystick = PInput.KeyToJoystick(key);
				_button = PInput.KeyToJoystickButton(key);
			}
		}

		public JoystickButton(string name, Joysticks joystick, JoystickButtons button) : base(name, PInput.JoystickInputToKey(joystick, button))
		{
			_joystick = joystick;
			_button = button;
		}

		public JoystickButton(string name, KeyCode key) : base(name, key)
		{
			_joystick = PInput.KeyToJoystick(key);
			_button = PInput.KeyToJoystickButton(key);
		}
	}
}