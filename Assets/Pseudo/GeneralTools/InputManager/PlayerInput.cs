using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal;

namespace Pseudo
{
	public class PlayerInput : PMonoBehaviour
	{
		[SerializeField]
		List<PlayerInputAction> actions = new List<PlayerInputAction>();

		public bool GetKeyDown(string action)
		{
			for (int i = 0; i < actions.Count; i++)
			{

			}

			return false;
		}

		public bool GetKeyUp(string action)
		{
			return false;
		}

		public bool GetKey(string action)
		{
			return false;
		}

		public float GetAxis(string action)
		{
			return 0f;
		}
	}
}