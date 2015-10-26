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
				_key = InputSystem.JoystickInputToKey(_joystick, _button);
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
				_key = InputSystem.JoystickInputToKey(_joystick, _button);
			}
		}

		public override KeyCode Key
		{
			get { return _key; }
			set
			{
				_key = value;
				_joystick = InputSystem.KeyToJoystick(_key);
				_button = InputSystem.KeyToJoystickButton(_key);
			}
		}

		public JoystickButton(string name, Joysticks joystick, JoystickButtons button) : base(name, InputSystem.JoystickInputToKey(joystick, button))
		{
			_joystick = joystick;
			_button = button;
		}

		public JoystickButton(string name, KeyCode key) : base(name, key)
		{
			_joystick = InputSystem.KeyToJoystick(key);
			_button = InputSystem.KeyToJoystickButton(key);
		}
	}
}