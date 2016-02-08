using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo
{
	[Serializable]
	public class Architect
	{
		public void CreateNewMap(string text, int width, int height)
		{
			PDebug.Log(text, width, height);
		}
	}
}
