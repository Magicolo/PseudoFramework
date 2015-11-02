using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal.Input;

namespace Pseudo.Internal.Tests
{
	public class InputManagerTests : PMonoBehaviour
	{
		void Update()
		{
			if (InputManager.Instance.GetKeyDown(InputManager.Players.Player1, "Jump"))
				PDebug.Log("Player1: Jump Pressed");

			if (InputManager.Instance.GetKey(InputManager.Players.Player2, "MotionX"))
				PDebug.Log("Player2: MotionX = " + InputManager.Instance.GetAxis(InputManager.Players.Player2, "MotionX"));
		}
	}
}