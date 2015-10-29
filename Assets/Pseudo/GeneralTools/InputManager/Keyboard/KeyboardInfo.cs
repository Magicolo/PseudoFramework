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
		List<KeyboardButton> buttons = new List<KeyboardButton>();
		[SerializeField]
		List<KeyboardAxis> axes = new List<KeyboardAxis>();

		Dictionary<string, List<KeyboardButton>> nameButtonDict;
		Dictionary<string, List<KeyboardButton>> NameButtonDict
		{
			get
			{
				if (nameButtonDict == null)
					BuildNameButtonDict();

				return nameButtonDict;
			}
		}

		Dictionary<string, List<KeyboardAxis>> nameAxisDict;
		Dictionary<string, List<KeyboardAxis>> NameAxisDict
		{
			get
			{
				if (nameAxisDict == null)
					BuildNameAxisDict();

				return nameAxisDict;
			}
		}

		public KeyboardInfo(string name, KeyboardButton[] buttons, KeyboardAxis[] axes) : base(name)
		{
			this.buttons = new List<KeyboardButton>(buttons);
			this.axes = new List<KeyboardAxis>(axes);

			BuildNameButtonDict();
			BuildNameAxisDict();
		}

		public KeyboardButton[] GetButtons()
		{
			return buttons.ToArray();
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
			this.buttons = new List<KeyboardButton>(buttons);
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
			buttons.Add(button);

			if (!NameButtonDict.ContainsKey(button.Name))
				NameButtonDict[button.Name] = new List<KeyboardButton>();

			NameButtonDict[button.Name].Add(button);
		}

		public void RemoveButton(KeyboardButton button)
		{
			buttons.Remove(button);

			if (NameButtonDict.ContainsKey(button.Name))
				NameButtonDict[button.Name].Remove(button);
		}

		public KeyboardAxis[] GetAxes()
		{
			return axes.ToArray();
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
			this.axes = new List<KeyboardAxis>(axes);

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
			axes.Add(axis);

			if (!NameAxisDict.ContainsKey(axis.Name))
			{
				NameAxisDict[axis.Name] = new List<KeyboardAxis>();
			}

			NameAxisDict[axis.Name].Add(axis);
		}

		public void RemoveAxis(KeyboardAxis axis)
		{
			axes.Remove(axis);

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
			nameButtonDict = new Dictionary<string, List<KeyboardButton>>();

			for (int i = 0; i < buttons.Count; i++)
			{
				KeyboardButton key = buttons[i];

				if (!nameButtonDict.ContainsKey(key.Name))
					nameButtonDict[key.Name] = new List<KeyboardButton>();

				nameButtonDict[key.Name].Add(key);
			}
		}

		void BuildNameAxisDict()
		{
			nameAxisDict = new Dictionary<string, List<KeyboardAxis>>();

			for (int i = 0; i < axes.Count; i++)
			{
				KeyboardAxis axis = axes[i];

				if (!nameAxisDict.ContainsKey(axis.Name))
					nameAxisDict[axis.Name] = new List<KeyboardAxis>();

				nameAxisDict[axis.Name].Add(axis);
			}
		}
	}
}