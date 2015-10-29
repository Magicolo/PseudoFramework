using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Internal;

namespace Pseudo
{
	[System.Serializable]
	public class KeyboardButton : ButtonBase
	{
		public KeyboardButton(string name, KeyCode key) : base(name, key)
		{

		}
	}
}