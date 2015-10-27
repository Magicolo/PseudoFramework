using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Pseudo
{
	public static class GUITextExtentions
	{
		public static void SetColor(this GUIText guiText, Color color, Channels channels)
		{
			guiText.color = guiText.color.SetValues(color, channels);
		}

		public static void SetColor(this GUIText guiText, float color, Channels channels)
		{
			guiText.SetColor(new Color(color, color, color, color), channels);
		}
	}
}

