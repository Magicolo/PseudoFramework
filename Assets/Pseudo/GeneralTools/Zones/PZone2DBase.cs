using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo
{
	public abstract class PZone2DBase : PMonoBehaviour
	{
		public abstract Vector2 GetRandomLocalPoint();
		public abstract Vector2 GetRandomWorldPoint();
	}
}