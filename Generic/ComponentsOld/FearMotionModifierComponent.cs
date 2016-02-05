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
		public TargetBase Target;
		[Tooltip("Rate at which the fear progresses over distance.")]
		public float Ratio = 1f;
		public float PerceptionDistance = 10f;
	}
}