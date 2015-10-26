using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Internal;

namespace Pseudo
{
	[System.Serializable]
	public class JoystickInfo : ControllerInfo
	{
		[SerializeField, PropertyField]
		Joysticks _joystick;
		public Joysticks Joystick
		{
			get { return _joystick; }
			set
			{
				_joystick = value;
				UpdateJoystick();
			}
		}

		[SerializeField]
		List<JoystickButton> _buttons = new List<JoystickButton>();
		[SerializeField]
		List<JoystickAxis> _axes = new List<JoystickAxis>();

		Dictionary<string, List<JoystickButton>> _nameButtonDict;
		Dictionary<string, List<JoystickButton>> NameButtonDict
		{
			get
			{
				if (_nameButtonDict == null)
					BuildNameButtonDict();

				return _nameButtonDict;
			}
		}

		Dictionary<string, List<JoystickAxis>> _nameAxisDict;
		Dictionary<string, List<JoystickAxis>> NameAxisDict
		{
			get
			{
				if (_nameAxisDict == null)
					BuildNameAxisDict();

				return _nameAxisDict;
			}
		}

		public JoystickInfo(string name, Joysticks joystick, JoystickButton[] buttons, JoystickAxis[] axes, IInputListener[] listeners) : base(name, listeners)
		{
			_joystick = joystick;
			_buttons = new List<JoystickButton>(buttons);
			_axes = new List<JoystickAxis>(axes);
		}

		public void UpdateInput()
		{
			if (!HasListeners())
				return;

			for (int i = 0; i < _buttons.Count; i++)
			{
				JoystickButton button = _buttons[i];
				if (Input.GetKeyDown(button.Key))
					SendButtonInput(button.Name, ButtonStates.Down);
				else if (Input.GetKeyUp(button.Key))
					SendButtonInput(button.Name, ButtonStates.Up);
				else if (Input.GetKey(button.Key))
					SendButtonInput(button.Name, ButtonStates.Pressed);
			}

			for (int i = 0; i < _axes.Count; i++)
			{
				JoystickAxis axis = _axes[i];
				float currentValue = Input.GetAxis(axis.Axis);

				if ((axis.LastValue != 0 && currentValue == 0) || currentValue - axis.LastValue != 0)
				{
					axis.LastValue = currentValue;

					SendAxisInput(axis.Name, currentValue);
				}
			}
		}

		public JoystickButton[] GetButtons()
		{
			return _buttons.ToArray();
		}

		public JoystickButton[] GetButtons(string buttonName)
		{
			return NameButtonDict[buttonName].ToArray();
		}

		public string[] GetButtonNames()
		{
			return NameButtonDict.GetKeyArray();
		}

		public void SetButtons(JoystickButton[] buttons)
		{
			_buttons = new List<JoystickButton>(buttons);

			BuildNameButtonDict();
		}

		public void CopyButtons(JoystickInfo info)
		{
			SetButtons(info.GetButtons());
		}

		public void SwitchButtons(JoystickInfo info)
		{
			JoystickButton[] otherButtons = info.GetButtons();

			info.SetButtons(GetButtons());
			SetButtons(otherButtons);
		}

		public void AddButton(JoystickButton button)
		{
			_buttons.Add(button);

			if (!NameButtonDict.ContainsKey(button.Name))
			{
				NameButtonDict[button.Name] = new List<JoystickButton>();
			}

			NameButtonDict[button.Name].Add(button);
		}

		public void RemoveButton(JoystickButton button)
		{
			_buttons.Remove(button);

			if (NameButtonDict.ContainsKey(button.Name))
			{
				NameButtonDict[button.Name].Remove(button);
			}
		}

		public JoystickAxis[] GetAxes()
		{
			return _axes.ToArray();
		}

		public JoystickAxis[] GetAxes(string axisName)
		{
			return NameAxisDict[axisName].ToArray();
		}

		public string[] GetAxisNames()
		{
			return NameAxisDict.GetKeyArray();
		}

		public void SetAxes(JoystickAxis[] axes)
		{
			_axes = new List<JoystickAxis>(axes);

			BuildNameAxisDict();
		}

		public void CopyAxes(JoystickInfo info)
		{
			SetAxes(info.GetAxes());
		}

		public void SwitchAxes(JoystickInfo info)
		{
			JoystickAxis[] otherAxes = info.GetAxes();

			info.SetAxes(GetAxes());
			SetAxes(otherAxes);
		}

		public void AddAxis(JoystickAxis axis)
		{
			_axes.Add(axis);

			if (!NameAxisDict.ContainsKey(axis.Name))
				NameAxisDict[axis.Name] = new List<JoystickAxis>();

			NameAxisDict[axis.Name].Add(axis);
		}

		public void RemoveAxis(JoystickAxis axis)
		{
			_axes.Remove(axis);

			if (NameAxisDict.ContainsKey(axis.Name))
			{
				NameAxisDict[axis.Name].Remove(axis);
			}
		}

		public void CopyInput(JoystickInfo info)
		{
			CopyButtons(info);
			CopyAxes(info);
		}

		public void SwitchInput(JoystickInfo info)
		{
			SwitchButtons(info);
			SwitchAxes(info);
		}

		void UpdateJoystick()
		{
			for (int i = 0; i < _buttons.Count; i++)
				_buttons[i].Joystick = Joystick;

			for (int i = 0; i < _axes.Count; i++)
				_axes[i].Joystick = Joystick;
		}

		void BuildNameButtonDict()
		{
			_nameButtonDict = new Dictionary<string, List<JoystickButton>>();

			for (int i = 0; i < _buttons.Count; i++)
			{
				JoystickButton button = _buttons[i];

				if (!_nameButtonDict.ContainsKey(button.Name))
					_nameButtonDict[button.Name] = new List<JoystickButton>();

				_nameButtonDict[button.Name].Add(button);
			}
		}

		void BuildNameAxisDict()
		{
			_nameAxisDict = new Dictionary<string, List<JoystickAxis>>();

			for (int i = 0; i < _axes.Count; i++)
			{
				JoystickAxis axis = _axes[i];

				if (!_nameAxisDict.ContainsKey(axis.Name))
					_nameAxisDict[axis.Name] = new List<JoystickAxis>();

				_nameAxisDict[axis.Name].Add(axis);
			}
		}
	}
}