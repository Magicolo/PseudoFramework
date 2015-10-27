using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Pseudo.Internal
{
	public class PScreenLoggerLine
	{
		public string Text;
		public Color Color;

		public PScreenLoggerLine(string text, Color color)
		{
			this.Text = text;
			this.Color = color;
		}
	}
}

