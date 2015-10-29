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
		Joysticks joystick;
		public Joysticks Joystick
		{
			get { return joystick; }
			set
			{
				joystick = value;
				UpdateJoystick();
			}
		}

		[SerializeField]
		List<JoystickButton> buttons = new List<JoystickButton>();
		[SerializeField]
		List<JoystickAxis> axes = new List<JoystickAxis>();

		Dictionary<string, List<JoystickButton>> nameButtonDict;
		Dictionary<string, List<JoystickButton>> NameButtonDict
		{
			get
			{
				if (nameButtonDict == null)
					BuildNameButtonDict();

				return nameButtonDict;
			}
		}

		Dictionary<string, List<JoystickAxis>> nameAxisDict;
		Dictionary<string, List<JoystickAxis>> NameAxisDict
		{
			get
			{
				if (nameAxisDict == null)
					BuildNameAxisDict();

				return nameAxisDict;
			}
		}

		public JoystickInfo(string name, Joysticks joystick, JoystickButton[] buttons, JoystickAxis[] axes) : base(name)
		{
			this.joystick = joystick;
			this.buttons = new List<JoystickButton>(buttons);
			this.axes = new List<JoystickAxis>(axes);
		}

		public JoystickButton[] GetButtons()
		{
			return buttons.ToArray();
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
			this.buttons = new List<JoystickButton>(buttons);

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
			buttons.Add(button);

			if (!NameButtonDict.ContainsKey(button.Name))
				NameButtonDict[button.Name] = new List<JoystickButton>();

			NameButtonDict[button.Name].Add(button);
		}

		public void RemoveButton(JoystickButton button)
		{
			buttons.Remove(button);

			if (NameButtonDict.ContainsKey(button.Name))
				NameButtonDict[button.Name].Remove(button);
		}

		public JoystickAxis[] GetAxes()
		{
			return axes.ToArray();
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
			this.axes = new List<JoystickAxis>(axes);

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
			axes.Add(axis);

			if (!NameAxisDict.ContainsKey(axis.Name))
				NameAxisDict[axis.Name] = new List<JoystickAxis>();

			NameAxisDict[axis.Name].Add(axis);
		}

		public void RemoveAxis(JoystickAxis axis)
		{
			axes.Remove(axis);

			if (NameAxisDict.ContainsKey(axis.Name))
				NameAxisDict[axis.Name].Remove(axis);
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
			for (int i = 0; i < buttons.Count; i++)
				buttons[i].Joystick = Joystick;

			for (int i = 0; i < axes.Count; i++)
				axes[i].Joystick = Joystick;
		}

		void BuildNameButtonDict()
		{
			nameButtonDict = new Dictionary<string, List<JoystickButton>>();

			for (int i = 0; i < buttons.Count; i++)
			{
				JoystickButton button = buttons[i];

				if (!nameButtonDict.ContainsKey(button.Name))
					nameButtonDict[button.Name] = new List<JoystickButton>();

				nameButtonDict[button.Name].Add(button);
			}
		}

		void BuildNameAxisDict()
		{
			nameAxisDict = new Dictionary<string, List<JoystickAxis>>();

			for (int i = 0; i < axes.Count; i++)
			{
				JoystickAxis axis = axes[i];

				if (!nameAxisDict.ContainsKey(axis.Name))
					nameAxisDict[axis.Name] = new List<JoystickAxis>();

				nameAxisDict[axis.Name].Add(axis);
			}
		}
	}
}