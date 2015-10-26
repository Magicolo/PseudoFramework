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
		protected string _name;
		public string Name { get { return _name; } }

		[SerializeField]
		protected KeyCode _key;
		public virtual KeyCode Key { get { return _key; } set { _key = value; } }

		public ButtonBase(string name, KeyCode key)
		{
			_name = name;
			_key = key;
		}

		public bool IsDown()
		{
			return Input.GetKeyDown(_key);
		}

		public bool IsUp()
		{
			return Input.GetKeyUp(_key);
		}

		public bool IsPressed()
		{
			return Input.GetKey(_key);
		}
	}
}