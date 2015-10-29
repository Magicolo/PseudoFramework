using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Internal;

namespace Pseudo
{
	[System.Serializable]
	public class KeyboardAxis : AxisBase
	{
		public KeyboardAxis(string name, string axis) : base(name, axis)
		{
		}
	}
}