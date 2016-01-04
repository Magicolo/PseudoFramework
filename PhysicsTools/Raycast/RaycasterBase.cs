using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo.Internal.Physics
{
	[ExecuteInEditMode]
	public abstract class RaycasterBase : PMonoBehaviour
	{
		public readonly List<RaycastHit> Hits = new List<RaycastHit>();

		public LayerMask Mask = UnityEngine.Physics.DefaultRaycastLayers;
		public RaycastHitModes HitMode = RaycastHitModes.FirstOfEach;
		public QueryTriggerInteraction HitTrigger = QueryTriggerInteraction.UseGlobal;
		public bool Draw = true;

		/// <summary>
		/// Updates the Raycaster and stores the results in the Hits list.
		/// </summary>
		/// <returns>If the raycaster has hit.</returns>
		public abstract bool Cast();

		void Update()
		{
			Cast();
		}
	}
}