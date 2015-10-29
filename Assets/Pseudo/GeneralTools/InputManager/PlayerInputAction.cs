using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo.Internal
{
	[Serializable]
	public class PlayerInputAction : INamable
	{
		string name;
		public string Name { get { return name; } set { name = value; } }

		public KeyboardButton[] KeyboardButtons;
		public KeyboardAxis[] KeyboardAxes;
		public JoystickButton[] JoystickButtons;
		public JoystickAxis[] JoystickAxes;

		public bool GetKeyDown()
		{
			for (int i = 0; i < KeyboardButtons.Length; i++)
			{
				if (KeyboardButtons[i].GetKeyDown())
					return true;
			}

			for (int i = 0; i < JoystickButtons.Length; i++)
			{
				if (JoystickButtons[i].GetKeyDown())
					return true;
			}

			return false;
		}

		public bool GetKeyUp()
		{
			for (int i = 0; i < KeyboardButtons.Length; i++)
			{
				if (KeyboardButtons[i].GetKeyUp())
					return true;
			}

			for (int i = 0; i < JoystickButtons.Length; i++)
			{
				if (JoystickButtons[i].GetKeyUp())
					return true;
			}

			return false;
		}

		public bool GetKey()
		{
			for (int i = 0; i < KeyboardButtons.Length; i++)
			{
				if (KeyboardButtons[i].GetKey())
					return true;
			}

			for (int i = 0; i < JoystickButtons.Length; i++)
			{
				if (JoystickButtons[i].GetKey())
					return true;
			}

			return false;
		}

		public float GetAxis()
		{


			return 0f;
		}
	}
}