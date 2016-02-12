using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo
{
	[RequireComponent(typeof(MotionComponent))]
	public abstract class MotionModifierComponentBase : ComponentBehaviour
	{
		public float Strength = 1f;
	}
}