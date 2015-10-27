using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Internal;

namespace Pseudo
{
	[System.Serializable]
	public class PKeyboardButton : PButtonBase
	{
		public PKeyboardButton(string name, KeyCode key) : base(name, key)
		{

		}
	}
}