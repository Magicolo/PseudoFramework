using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Internal;

namespace Pseudo.Input.Internal
{
	[System.Serializable]
	public class KeyboardButton
	{
		[SerializeField]
		protected KeyCode key;
		public KeyCode Key
		{
			get { return key; }
			set { key = value; }
		}

		public KeyboardButton(KeyCode key)
		{
			this.key = key;
		}

		public bool GetKeyDown()
		{
			return UnityEngine.Input.GetKeyDown(key);
		}

		public bool GetKeyUp()
		{
			return UnityEngine.Input.GetKeyUp(key);
		}

		public bool GetKey()
		{
			return UnityEngine.Input.GetKey(key);
		}
	}
}