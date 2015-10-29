using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo.Internal
{
	public class ButtonBase
	{
		[SerializeField]
		protected string name;
		public string Name { get { return name; } }

		[SerializeField]
		protected KeyCode key;
		public virtual KeyCode Key { get { return key; } set { key = value; } }

		public ButtonBase(string name, KeyCode key)
		{
			this.name = name;
			this.key = key;
		}

		public bool GetKeyDown()
		{
			return Input.GetKeyDown(key);
		}

		public bool GetKeyUp()
		{
			return Input.GetKeyUp(key);
		}

		public bool GetKey()
		{
			return Input.GetKey(key);
		}
	}
}