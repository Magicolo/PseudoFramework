using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using UnityEngine.Assertions;

namespace Pseudo
{
	public class GameObjectComponent : ComponentBase
	{
		public readonly GameObject GameObject;

		public GameObjectComponent(GameObject gameObject)
		{
			GameObject = gameObject;
		}
	}
}