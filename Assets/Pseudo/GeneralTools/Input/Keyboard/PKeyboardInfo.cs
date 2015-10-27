using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Internal;

namespace Pseudo
{
	[System.Serializable]
	public class PKeyboardInfo : PControllerInfo
	{
		[SerializeField]
		List<PKeyboardButton> buttons = new List<PKeyboardButton>();
		[SerializeField]
		List<PKeyboardAxis> axes = new List<PKeyboardAxis>();

		Dictionary<string, List<PKeyboardButton>> nameButtonDict;
		Dictionary<string, List<PKeyboardButton>> NameButtonDict
		{
			get
			{
				if (nameButtonDict == null)
					BuildNameButtonDict();

				return nameButtonDict;
			}
		}

		Dictionary<string, List<PKeyboardAxis>> nameAxisDict;
		Dictionary<string, List<PKeyboardAxis>> NameAxisDict
		{
			get
			{
				if (nameAxisDict == null)
					BuildNameAxisDict();

				return nameAxisDict;
			}
		}

		public PKeyboardInfo(string name, PKeyboardButton[] buttons, PKeyboardAxis[] axes) : base(name)
		{
			this.buttons = new List<PKeyboardButton>(buttons);
			this.axes = new List<PKeyboardAxis>(axes);

			BuildNameButtonDict();
			BuildNameAxisDict();
		}

		public PKeyboardButton[] GetButtons()
		{
			return buttons.ToArray();
		}

		public PKeyboardButton[] GetButtons(string buttonName)
		{
			return NameButtonDict[buttonName].ToArray();
		}

		public string[] GetButtonNames()
		{
			return NameButtonDict.GetKeyArray();
		}

		public void SetButtons(PKeyboardButton[] buttons)
		{
			this.buttons = new List<PKeyboardButton>(buttons);
			BuildNameButtonDict();
		}

		public void CopyButtons(PKeyboardInfo info)
		{
			SetButtons(info.GetButtons());
		}

		public void SwitchButtons(PKeyboardInfo info)
		{
			PKeyboardButton[] otherButtons = info.GetButtons();
			info.SetButtons(GetButtons());
			SetButtons(otherButtons);
		}

		public void AddButton(PKeyboardButton button)
		{
			buttons.Add(button);

			if (!NameButtonDict.ContainsKey(button.Name))
				NameButtonDict[button.Name] = new List<PKeyboardButton>();

			NameButtonDict[button.Name].Add(button);
		}

		public void RemoveButton(PKeyboardButton button)
		{
			buttons.Remove(button);

			if (NameButtonDict.ContainsKey(button.Name))
				NameButtonDict[button.Name].Remove(button);
		}

		public PKeyboardAxis[] GetAxes()
		{
			return axes.ToArray();
		}

		public PKeyboardAxis[] GetAxes(string axisName)
		{
			return NameAxisDict[axisName].ToArray();
		}

		public string[] GetAxisNames()
		{
			return NameAxisDict.GetKeyArray();
		}

		public void SetAxes(PKeyboardAxis[] axes)
		{
			this.axes = new List<PKeyboardAxis>(axes);

			BuildNameAxisDict();
		}

		public void CopyAxes(PKeyboardInfo info)
		{
			SetAxes(info.GetAxes());
		}

		public void SwitchAxes(PKeyboardInfo info)
		{
			PKeyboardAxis[] otherAxes = info.GetAxes();

			info.SetAxes(GetAxes());
			SetAxes(otherAxes);
		}

		public void AddAxis(PKeyboardAxis axis)
		{
			axes.Add(axis);

			if (!NameAxisDict.ContainsKey(axis.Name))
			{
				NameAxisDict[axis.Name] = new List<PKeyboardAxis>();
			}

			NameAxisDict[axis.Name].Add(axis);
		}

		public void RemoveAxis(PKeyboardAxis axis)
		{
			axes.Remove(axis);

			if (NameAxisDict.ContainsKey(axis.Name))
			{
				NameAxisDict[axis.Name].Remove(axis);
			}
		}

		public void CopyInput(PKeyboardInfo info)
		{
			CopyButtons(info);
			CopyAxes(info);
		}

		public void SwitchInput(PKeyboardInfo info)
		{
			SwitchButtons(info);
			SwitchAxes(info);
		}

		void BuildNameButtonDict()
		{
			nameButtonDict = new Dictionary<string, List<PKeyboardButton>>();

			for (int i = 0; i < buttons.Count; i++)
			{
				PKeyboardButton key = buttons[i];

				if (!nameButtonDict.ContainsKey(key.Name))
					nameButtonDict[key.Name] = new List<PKeyboardButton>();

				nameButtonDict[key.Name].Add(key);
			}
		}

		void BuildNameAxisDict()
		{
			nameAxisDict = new Dictionary<string, List<PKeyboardAxis>>();

			for (int i = 0; i < axes.Count; i++)
			{
				PKeyboardAxis axis = axes[i];

				if (!nameAxisDict.ContainsKey(axis.Name))
					nameAxisDict[axis.Name] = new List<PKeyboardAxis>();

				nameAxisDict[axis.Name].Add(axis);
			}
		}
	}
}