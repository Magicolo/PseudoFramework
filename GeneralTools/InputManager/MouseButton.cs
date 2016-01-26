using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo.Internal.Input
{
	[Serializable]
	public class MouseButton
	{
		[SerializeField]
		protected InputManager.MouseButtons button = InputManager.MouseButtons.LeftClick;
		protected KeyCode key;

		public InputManager.MouseButtons Button
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

		public MouseButton(InputManager.MouseButtons button)
		{
			this.button = button;
			key = (KeyCode)button;
		}

		public MouseButton(KeyCode key)
		{
			this.key = key;
			button = (InputManager.MouseButtons)key;
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