using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo
{
	public class SeekMotionModifierComponent : MotionModifierComponentBase
	{
		public TargetBase Target;
		public float PerceptionDistance = 10f;
		public float SlowDistance = 3f;
		public float StopDistance = 1f;
	}
}