using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Internal;

namespace Pseudo
{
	[System.Serializable]
	public class KeyboardInfo : ControllerInfo
	{
		[SerializeField]
		List<KeyboardButton> _buttons = new List<KeyboardButton>();
		[SerializeField]
		List<KeyboardAxis> _axes = new List<KeyboardAxis>();

		Dictionary<string, List<KeyboardButton>> _nameButtonDict;
		Dictionary<string, List<KeyboardButton>> NameButtonDict
		{
			get
			{
				if (_nameButtonDict == null)
				{
					BuildNameButtonDict();
				}

				return _nameButtonDict;
			}
		}

		Dictionary<string, List<KeyboardAxis>> _nameAxisDict;
		Dictionary<string, List<KeyboardAxis>> NameAxisDict
		{
			get
			{
				if (_nameAxisDict == null)
				{
					BuildNameAxisDict();
				}

				return _nameAxisDict;
			}
		}

		public KeyboardInfo(string name, KeyboardButton[] buttons, KeyboardAxis[] axes, IInputListener[] listeners) : base(name, listeners)
		{
			_buttons = new List<KeyboardButton>(buttons);
			_axes = new List<KeyboardAxis>(axes);

			BuildNameButtonDict();
			BuildNameAxisDict();
		}

		public void UpdateInput()
		{
			if (!HasListeners())
				return;

			for (int i = 0; i < _buttons.Count; i++)
			{
				KeyboardButton button = _buttons[i];

				if (Input.GetKeyDown(button.Key))
					SendButtonInput(button.Name, ButtonStates.Down);
				else if (Input.GetKeyUp(button.Key))
					SendButtonInput(button.Name, ButtonStates.Up);
				else if (Input.GetKey(button.Key))
					SendButtonInput(button.Name, ButtonStates.Pressed);
			}

			for (int i = 0; i < _axes.Count; i++)
			{
				KeyboardAxis axis = _axes[i];
				float currentValue = Input.GetAxis(axis.Axis);

				if ((axis.LastValue != 0 && currentValue == 0) || currentValue - axis.LastValue != 0)
				{
					axis.LastValue = currentValue;

					SendAxisInput(axis.Name, currentValue);
				}
			}
		}

		public KeyboardButton[] GetButtons()
		{
			return _buttons.ToArray();
		}

		public KeyboardButton[] GetButtons(string buttonName)
		{
			return NameButtonDict[buttonName].ToArray();
		}

		public string[] GetButtonNames()
		{
			return NameButtonDict.GetKeyArray();
		}

		public void SetButtons(KeyboardButton[] buttons)
		{
			_buttons = new List<KeyboardButton>(buttons);
			BuildNameButtonDict();
		}

		public void CopyButtons(KeyboardInfo info)
		{
			SetButtons(info.GetButtons());
		}

		public void SwitchButtons(KeyboardInfo info)
		{
			KeyboardButton[] otherButtons = info.GetButtons();
			info.SetButtons(GetButtons());
			SetButtons(otherButtons);
		}

		public void AddButton(KeyboardButton button)
		{
			_buttons.Add(button);

			if (!NameButtonDict.ContainsKey(button.Name))
				NameButtonDict[button.Name] = new List<KeyboardButton>();

			NameButtonDict[button.Name].Add(button);
		}

		public void RemoveButton(KeyboardButton button)
		{
			_buttons.Remove(button);

			if (NameButtonDict.ContainsKey(button.Name))
				NameButtonDict[button.Name].Remove(button);
		}

		public KeyboardAxis[] GetAxes()
		{
			return _axes.ToArray();
		}

		public KeyboardAxis[] GetAxes(string axisName)
		{
			return NameAxisDict[axisName].ToArray();
		}

		public string[] GetAxisNames()
		{
			return NameAxisDict.GetKeyArray();
		}

		public void SetAxes(KeyboardAxis[] axes)
		{
			this._axes = new List<KeyboardAxis>(axes);

			BuildNameAxisDict();
		}

		public void CopyAxes(KeyboardInfo info)
		{
			SetAxes(info.GetAxes());
		}

		public void SwitchAxes(KeyboardInfo info)
		{
			KeyboardAxis[] otherAxes = info.GetAxes();

			info.SetAxes(GetAxes());
			SetAxes(otherAxes);
		}

		public void AddAxis(KeyboardAxis axis)
		{
			_axes.Add(axis);

			if (!NameAxisDict.ContainsKey(axis.Name))
			{
				NameAxisDict[axis.Name] = new List<KeyboardAxis>();
			}

			NameAxisDict[axis.Name].Add(axis);
		}

		public void RemoveAxis(KeyboardAxis axis)
		{
			_axes.Remove(axis);

			if (NameAxisDict.ContainsKey(axis.Name))
			{
				NameAxisDict[axis.Name].Remove(axis);
			}
		}

		public void CopyInput(KeyboardInfo info)
		{
			CopyButtons(info);
			CopyAxes(info);
		}

		public void SwitchInput(KeyboardInfo info)
		{
			SwitchButtons(info);
			SwitchAxes(info);
		}

		void BuildNameButtonDict()
		{
			_nameButtonDict = new Dictionary<string, List<KeyboardButton>>();

			foreach (KeyboardButton key in _buttons)
			{
				if (!_nameButtonDict.ContainsKey(key.Name))
				{
					_nameButtonDict[key.Name] = new List<KeyboardButton>();
				}

				_nameButtonDict[key.Name].Add(key);
			}
		}

		void BuildNameAxisDict()
		{
			_nameAxisDict = new Dictionary<string, List<KeyboardAxis>>();

			foreach (KeyboardAxis axis in _axes)
			{
				if (!_nameAxisDict.ContainsKey(axis.Name))
				{
					_nameAxisDict[axis.Name] = new List<KeyboardAxis>();
				}

				_nameAxisDict[axis.Name].Add(axis);
			}
		}
	}
}