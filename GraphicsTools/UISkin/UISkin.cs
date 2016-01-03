using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Pseudo
{
	[System.Serializable]
	public class UISkin : MonoBehaviour
	{
		public Color SelectedButtonBackground;
		public Color DisabledButtonBackground = Color.gray;
		public Color EnabledButtonBackground = Color.white;

		public void Disable(params Button[] buttons)
		{
			for (int i = 0; i < buttons.Length; i++)
			{
				buttons[i].enabled = false;
				buttons[i].GetComponent<Image>().color = DisabledButtonBackground;
			}
		}

		public void Enabled(params Button[] buttons)
		{
			for (int i = 0; i < buttons.Length; i++)
			{
				buttons[i].enabled = true;
				buttons[i].GetComponent<Image>().color = EnabledButtonBackground;
			}
		}

		public void Select(params Button[] buttons)
		{
			for (int i = 0; i < buttons.Length; i++)
			{
				buttons[i].enabled = true;
				buttons[i].GetComponent<Image>().color = SelectedButtonBackground;
			}
		}

		public void SetEnabled(Button button, bool enabled)
		{
			if (enabled)
				Enabled(button);
			else
				Disable(button);
		}
	}

}
