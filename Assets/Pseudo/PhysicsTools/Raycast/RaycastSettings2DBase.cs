using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo.Internal.Physics
{
	[Serializable]
	public class RaycastSettings2DBase
	{
		public Vector2 Offset;
		public RaycastHitModes HitMode = RaycastHitModes.FirstOfEach;
		public LayerMask Mask;
		public bool Draw = true;
	}
}