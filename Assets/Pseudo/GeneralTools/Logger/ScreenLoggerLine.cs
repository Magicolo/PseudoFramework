﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Pseudo.Internal {
	public class ScreenLoggerLine {

		public string text;
		public Color color;
	
		public ScreenLoggerLine(string text, Color color) {
			this.text = text;
			this.color = color;
		}
	}
}

