using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Pseudo.Internal
{
	public class ScreenLoggerLine
	{
		public string Text;
		public Color Color;

		public ScreenLoggerLine(string text, Color color)
		{
			this.Text = text;
			this.Color = color;
		}
	}
}

