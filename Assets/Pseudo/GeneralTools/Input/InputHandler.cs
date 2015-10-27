using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

public class InputHandler : PMonoBehaviour
{
	public List<KeyboardButton> KeyboardButtons;
	public List<KeyboardAxis> KeyboardAxes;

	[SerializeField, Empty(BeforeSeparator = true)]
	Joysticks joystick;
	public Joysticks Joystick { get { return joystick; } }
	public List<JoystickButton> JoystickButtons;
	public List<JoystickAxis> JoystickAxes;

	void Awake()
	{
		InitializeJoystickInput();
	}

	void InitializeJoystickInput()
	{
		for (int i = 0; i < JoystickButtons.Count; i++)
			JoystickButtons[i].Joystick = joystick;

		for (int i = 0; i < JoystickAxes.Count; i++)
			JoystickAxes[i].Joystick = joystick;
	}

	public bool GetButtonDown(string name)
	{
		for (int i = 0; i < KeyboardButtons.Count; i++)
		{
			KeyboardButton button = KeyboardButtons[i];

			if (button.Name == name && button.IsDown())
				return true;
		}

		for (int i = 0; i < JoystickButtons.Count; i++)
		{
			JoystickButton button = JoystickButtons[i];

			if (button.Name == name && button.IsDown())
				return true;
		}

		return false;
	}

	public bool GetButtonUp(string name)
	{
		for (int i = 0; i < KeyboardButtons.Count; i++)
		{
			KeyboardButton button = KeyboardButtons[i];

			if (button.Name == name && button.IsUp())
				return true;
		}

		for (int i = 0; i < JoystickButtons.Count; i++)
		{
			JoystickButton button = JoystickButtons[i];

			if (button.Name == name && button.IsUp())
				return true;
		}

		return false;
	}

	public bool GetButtonPressed(string name)
	{
		for (int i = 0; i < KeyboardButtons.Count; i++)
		{
			KeyboardButton button = KeyboardButtons[i];

			if (button.Name == name && button.IsPressed())
				return true;
		}

		for (int i = 0; i < JoystickButtons.Count; i++)
		{
			JoystickButton button = JoystickButtons[i];

			if (button.Name == name && button.IsPressed())
				return true;
		}

		return false;
	}

	public float GetAxis(string name)
	{
		for (int i = 0; i < KeyboardAxes.Count; i++)
		{
			KeyboardAxis axis = KeyboardAxes[i];

			if (axis.Name == name)
			{
				float axisValue = axis.GetValue();

				if (axisValue != 0f)
					return axisValue;
			}
		}

		for (int i = 0; i < JoystickAxes.Count; i++)
		{
			JoystickAxis axis = JoystickAxes[i];

			if (axis.Name == name)
			{
				float axisValue = axis.GetValue();

				if (axisValue != 0f)
					return axis.GetValue();
			}
		}

		return 0f;
	}

	public void SetJoystick(Joysticks joystick)
	{
		this.joystick = joystick;

		InitializeJoystickInput();
	}
}
