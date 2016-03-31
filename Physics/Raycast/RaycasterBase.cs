using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.EntityFramework;

namespace Pseudo.Physics.Internal
{
	public abstract class RaycasterBase : ComponentBehaviourBase, IRaycaster
	{
		public readonly List<RaycastHit> Hits = new List<RaycastHit>();

		public LayerMask Mask = UnityEngine.Physics.DefaultRaycastLayers;
		public QueryTriggerInteraction HitTrigger = QueryTriggerInteraction.UseGlobal;
		public bool Draw = true;

		bool hitTrigger;

		/// <summary>
		/// Updates the Raycaster and stores the results in the Hits list.
		/// </summary>
		/// <returns>If the raycaster has hit.</returns>
		public bool Cast()
		{
			BeginCast();
			UpdateCast();
			EndCast();

			return Hits.Count > 0;
		}

		void BeginCast()
		{
			Hits.Clear();
			hitTrigger = UnityEngine.Physics.queriesHitTriggers;

			switch (HitTrigger)
			{
				case QueryTriggerInteraction.Ignore:
					UnityEngine.Physics.queriesHitTriggers = false;
					break;
				case QueryTriggerInteraction.Collide:
					UnityEngine.Physics.queriesHitTriggers = true;
					break;
			}
		}

		void EndCast()
		{
			UnityEngine.Physics.queriesHitTriggers = hitTrigger;
		}

		void OnDrawGizmos()
		{
			if (!Application.isPlaying)
				Cast();
		}

		protected abstract void UpdateCast();
	}
}