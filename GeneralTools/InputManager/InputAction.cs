using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal.Input;

namespace Pseudo
{
	[Serializable]
	public class InputAction
	{
		[SerializeField]
		string name;
		public string Name { get { return name; } }

		public List<MouseButton> MouseButtons = new List<MouseButton>();
		public List<MouseAxis> MouseAxes = new List<MouseAxis>();
		public List<KeyboardButton> KeyboardButtons = new List<KeyboardButton>();
		public List<KeyboardAxis> KeyboardAxes = new List<KeyboardAxis>();
		public List<JoystickButton> JoystickButtons = new List<JoystickButton>();
		public List<JoystickAxis> JoystickAxes = new List<JoystickAxis>();

		public InputAction(string name)
		{
			this.name = name;
		}

		public bool GetKeyDown()
		{
			for (int i = 0; i < MouseButtons.Count; i++)
			{
				if (MouseButtons[i].GetKeyDown())
					return true;
			}

			for (int i = 0; i < KeyboardButtons.Count; i++)
			{
				if (KeyboardButtons[i].GetKeyDown())
					return true;
			}

			for (int i = 0; i < JoystickButtons.Count; i++)
			{
				if (JoystickButtons[i].GetKeyDown())
					return true;
			}

			for (int i = 0; i < KeyboardAxes.Count; i++)
			{
				if (KeyboardAxes[i].GetAxisDown())
					return true;
			}

			for (int i = 0; i < JoystickAxes.Count; i++)
			{
				if (JoystickAxes[i].GetAxisDown())
					return true;
			}

			return false;
		}

		public bool GetKeyDown(Vector2 relativeScreenPosition)
		{
			for (int i = 0; i < MouseAxes.Count; i++)
			{
				if (MouseAxes[i].GetAxisDown(relativeScreenPosition))
					return true;
			}

			return GetKeyDown();
		}

		public bool GetKeyUp()
		{
			for (int i = 0; i < MouseButtons.Count; i++)
			{
				if (MouseButtons[i].GetKeyUp())
					return true;
			}

			for (int i = 0; i < KeyboardButtons.Count; i++)
			{
				if (KeyboardButtons[i].GetKeyUp())
					return true;
			}

			for (int i = 0; i < JoystickButtons.Count; i++)
			{
				if (JoystickButtons[i].GetKeyUp())
					return true;
			}

			for (int i = 0; i < KeyboardAxes.Count; i++)
			{
				if (KeyboardAxes[i].GetAxisUp())
					return true;
			}

			for (int i = 0; i < JoystickAxes.Count; i++)
			{
				if (JoystickAxes[i].GetAxisUp())
					return true;
			}

			return false;
		}

		public bool GetKeyUp(Vector2 relativeScreenPosition)
		{
			for (int i = 0; i < MouseAxes.Count; i++)
			{
				if (MouseAxes[i].GetAxisUp(relativeScreenPosition))
					return true;
			}

			return GetKeyUp();
		}

		public bool GetKey()
		{
			for (int i = 0; i < MouseButtons.Count; i++)
			{
				if (MouseButtons[i].GetKey())
					return true;
			}

			for (int i = 0; i < KeyboardButtons.Count; i++)
			{
				if (KeyboardButtons[i].GetKey())
					return true;
			}

			for (int i = 0; i < JoystickButtons.Count; i++)
			{
				if (JoystickButtons[i].GetKey())
					return true;
			}

			for (int i = 0; i < KeyboardAxes.Count; i++)
			{
				if (KeyboardAxes[i].GetAxis())
					return true;
			}

			for (int i = 0; i < JoystickAxes.Count; i++)
			{
				if (JoystickAxes[i].GetAxis())
					return true;
			}

			return false;
		}

		public bool GetKey(Vector2 relativeScreenPosition)
		{
			for (int i = 0; i < MouseAxes.Count; i++)
			{
				if (MouseAxes[i].GetAxis(relativeScreenPosition))
					return true;
			}

			return GetKey();
		}

		public float GetAxis()
		{
			float value = 0f;

			for (int i = 0; i < KeyboardAxes.Count; i++)
				value += KeyboardAxes[i].GetValue();

			for (int i = 0; i < JoystickAxes.Count; i++)
				value += JoystickAxes[i].GetValue();

			return value;
		}

		public float GetAxis(Vector2 relativeScreenPosition)
		{
			float value = 0f;

			for (int i = 0; i < MouseAxes.Count; i++)
				value += MouseAxes[i].GetValue(relativeScreenPosition);

			return value + GetAxis();
		}

		public override string ToString()
		{
			return string.Format("{0}({1})", GetType().Name, name);
		}
	}
}