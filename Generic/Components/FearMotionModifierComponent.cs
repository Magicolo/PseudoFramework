using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo
{
	public class FearMotionModifierComponent : MotionModifierComponentBase
	{
		public TargetComponentBase Target;
		public float Strength = 1f;
		public float SlowDistance = 3f;
		public float StopDistance = 1f;
	}
}