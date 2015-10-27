using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo.Internal.Physics
{
	[Serializable]
	public class RaycastSettingsBase
	{
		public Vector3 Offset;
		public RaycastHitModes HitMode = RaycastHitModes.FirstOfEach;
		public QueryTriggerInteraction HitTrigger = QueryTriggerInteraction.UseGlobal;
		public LayerMask Mask;
		public bool Draw = true;
	}
}