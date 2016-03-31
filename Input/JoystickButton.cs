using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Internal;

namespace Pseudo.Input.Internal
{
	[System.Serializable]
	public class JoystickButton
	{
		[SerializeField]
		protected Joysticks joystick;
		[SerializeField]
		protected JoystickButtons button;
		protected KeyCode key;

		public Joysticks Joystick
		{
			get { return joystick; }
			set
			{
				if (joystick != value)
				{
					joystick = value;
					key = InputUtility.JoystickInputToKey(joystick, button);
				}
			}
		}
		public JoystickButtons Button
		{
			get { return button; }
			set
			{
				if (button != value)
				{
					button = value;
					key = InputUtility.JoystickInputToKey(joystick, button);
				}
			}
		}

		protected KeyCode Key
		{
			get
			{
				if (key == KeyCode.None)
					key = InputUtility.JoystickInputToKey(joystick, button);

				return key;
			}
		}

		public JoystickButton(Joysticks joystick, JoystickButtons button)
		{
			this.joystick = joystick;
			this.button = button;
			this.key = InputUtility.JoystickInputToKey(joystick, button);
		}

		public JoystickButton(KeyCode key)
		{
			this.key = key;
			this.joystick = InputUtility.KeyToJoystick(key);
			this.button = InputUtility.KeyToJoystickButton(key);
		}

		public virtual bool GetKeyDown()
		{
			return UnityEngine.Input.GetKeyDown(Key);
		}

		public virtual bool GetKeyUp()
		{
			return UnityEngine.Input.GetKeyUp(Key);
		}

		public virtual bool GetKey()
		{
			return UnityEngine.Input.GetKey(Key);
		}
	}
}