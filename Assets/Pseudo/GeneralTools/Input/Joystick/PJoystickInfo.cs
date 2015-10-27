using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Internal;

namespace Pseudo
{
	[System.Serializable]
	public class PJoystickInfo : PControllerInfo
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
		List<PJoystickButton> buttons = new List<PJoystickButton>();
		[SerializeField]
		List<PJoystickAxis> axes = new List<PJoystickAxis>();

		Dictionary<string, List<PJoystickButton>> nameButtonDict;
		Dictionary<string, List<PJoystickButton>> NameButtonDict
		{
			get
			{
				if (nameButtonDict == null)
					BuildNameButtonDict();

				return nameButtonDict;
			}
		}

		Dictionary<string, List<PJoystickAxis>> nameAxisDict;
		Dictionary<string, List<PJoystickAxis>> NameAxisDict
		{
			get
			{
				if (nameAxisDict == null)
					BuildNameAxisDict();

				return nameAxisDict;
			}
		}

		public PJoystickInfo(string name, Joysticks joystick, PJoystickButton[] buttons, PJoystickAxis[] axes) : base(name)
		{
			this.joystick = joystick;
			this.buttons = new List<PJoystickButton>(buttons);
			this.axes = new List<PJoystickAxis>(axes);
		}

		public PJoystickButton[] GetButtons()
		{
			return buttons.ToArray();
		}

		public PJoystickButton[] GetButtons(string buttonName)
		{
			return NameButtonDict[buttonName].ToArray();
		}

		public string[] GetButtonNames()
		{
			return NameButtonDict.GetKeyArray();
		}

		public void SetButtons(PJoystickButton[] buttons)
		{
			this.buttons = new List<PJoystickButton>(buttons);

			BuildNameButtonDict();
		}

		public void CopyButtons(PJoystickInfo info)
		{
			SetButtons(info.GetButtons());
		}

		public void SwitchButtons(PJoystickInfo info)
		{
			PJoystickButton[] otherButtons = info.GetButtons();

			info.SetButtons(GetButtons());
			SetButtons(otherButtons);
		}

		public void AddButton(PJoystickButton button)
		{
			buttons.Add(button);

			if (!NameButtonDict.ContainsKey(button.Name))
				NameButtonDict[button.Name] = new List<PJoystickButton>();

			NameButtonDict[button.Name].Add(button);
		}

		public void RemoveButton(PJoystickButton button)
		{
			buttons.Remove(button);

			if (NameButtonDict.ContainsKey(button.Name))
				NameButtonDict[button.Name].Remove(button);
		}

		public PJoystickAxis[] GetAxes()
		{
			return axes.ToArray();
		}

		public PJoystickAxis[] GetAxes(string axisName)
		{
			return NameAxisDict[axisName].ToArray();
		}

		public string[] GetAxisNames()
		{
			return NameAxisDict.GetKeyArray();
		}

		public void SetAxes(PJoystickAxis[] axes)
		{
			this.axes = new List<PJoystickAxis>(axes);

			BuildNameAxisDict();
		}

		public void CopyAxes(PJoystickInfo info)
		{
			SetAxes(info.GetAxes());
		}

		public void SwitchAxes(PJoystickInfo info)
		{
			PJoystickAxis[] otherAxes = info.GetAxes();

			info.SetAxes(GetAxes());
			SetAxes(otherAxes);
		}

		public void AddAxis(PJoystickAxis axis)
		{
			axes.Add(axis);

			if (!NameAxisDict.ContainsKey(axis.Name))
				NameAxisDict[axis.Name] = new List<PJoystickAxis>();

			NameAxisDict[axis.Name].Add(axis);
		}

		public void RemoveAxis(PJoystickAxis axis)
		{
			axes.Remove(axis);

			if (NameAxisDict.ContainsKey(axis.Name))
				NameAxisDict[axis.Name].Remove(axis);
		}

		public void CopyInput(PJoystickInfo info)
		{
			CopyButtons(info);
			CopyAxes(info);
		}

		public void SwitchInput(PJoystickInfo info)
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
			nameButtonDict = new Dictionary<string, List<PJoystickButton>>();

			for (int i = 0; i < buttons.Count; i++)
			{
				PJoystickButton button = buttons[i];

				if (!nameButtonDict.ContainsKey(button.Name))
					nameButtonDict[button.Name] = new List<PJoystickButton>();

				nameButtonDict[button.Name].Add(button);
			}
		}

		void BuildNameAxisDict()
		{
			nameAxisDict = new Dictionary<string, List<PJoystickAxis>>();

			for (int i = 0; i < axes.Count; i++)
			{
				PJoystickAxis axis = axes[i];

				if (!nameAxisDict.ContainsKey(axis.Name))
					nameAxisDict[axis.Name] = new List<PJoystickAxis>();

				nameAxisDict[axis.Name].Add(axis);
			}
		}
	}
}