using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo.Input.Internal
{
	[Serializable]
	public class MouseButton
	{
		[SerializeField]
		protected MouseButtons button = MouseButtons.LeftClick;
		protected KeyCode key;

		public MouseButtons Button
		{
			get { return button; }
			set
			{
				if (button != value)
				{
					button = value;
					key = (KeyCode)button;
				}
			}
		}

		protected KeyCode Key
		{
			get
			{
				if (key == KeyCode.None)
					key = (KeyCode)button;

				return key;
			}
		}

		public MouseButton(MouseButtons button)
		{
			this.button = button;
			key = (KeyCode)button;
		}

		public MouseButton(KeyCode key)
		{
			this.key = key;
			button = (MouseButtons)key;
		}

		public virtual bool GetKeyDown()
		{
			return UnityEngine.Input.GetKeyDown(Key);
		}

		public virtual bool GetKeyUp()
		{
			return UnityEngine.Input.GetKeyUp(Key);
		}

		public virtual bool GetKey()
		{
			return UnityEngine.Input.GetKey(Key);
		}
	}
}