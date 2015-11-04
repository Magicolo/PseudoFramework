﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal.Input;

namespace Pseudo
{
	[Serializable]
	public class InputAction : INamable
	{
		[SerializeField]
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

			for (int i = 0; i < KeyboardAxes.Length; i++)
			{
				if (KeyboardAxes[i].GetAxisDown())
					return true;
			}

			for (int i = 0; i < JoystickAxes.Length; i++)
			{
				if (JoystickAxes[i].GetAxisDown())
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

			for (int i = 0; i < KeyboardAxes.Length; i++)
			{
				if (KeyboardAxes[i].GetAxisUp())
					return true;
			}

			for (int i = 0; i < JoystickAxes.Length; i++)
			{
				if (JoystickAxes[i].GetAxisUp())
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

			for (int i = 0; i < KeyboardAxes.Length; i++)
			{
				if (KeyboardAxes[i].GetAxis())
					return true;
			}

			for (int i = 0; i < JoystickAxes.Length; i++)
			{
				if (JoystickAxes[i].GetAxis())
					return true;
			}

			return false;
		}

		public float GetAxis()
		{
			float value = 0f;

			for (int i = 0; i < KeyboardAxes.Length; i++)
				value += KeyboardAxes[i].GetValue();

			for (int i = 0; i < JoystickAxes.Length; i++)
				value += JoystickAxes[i].GetValue();

			return value;
		}
	}
}