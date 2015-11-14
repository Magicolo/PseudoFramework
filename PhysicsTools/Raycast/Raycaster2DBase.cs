using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo.Internal.Physics
{
	public abstract class Raycaster2DBase : PMonoBehaviour
	{
		public readonly List<RaycastHit2D> Hits = new List<RaycastHit2D>();

		public LayerMask Mask = Physics2D.DefaultRaycastLayers;
		public QueryTriggerInteraction HitTrigger = QueryTriggerInteraction.UseGlobal;
		public bool Draw = true;

		bool hitTrigger;

		/// <summary>
		/// Updates the Raycaster and stores the results in the Hits list.
		/// </summary>
		/// <returns>If the raycaster has hit.</returns>
		public virtual bool Cast()
		{
			BeginCast();
			UpdateCast();
			EndCast();

			return Hits.Count > 0;
		}

		protected abstract void UpdateCast();

		protected virtual void BeginCast()
		{
			hitTrigger = Physics2D.queriesHitTriggers;

			switch (HitTrigger)
			{
				case QueryTriggerInteraction.Ignore:
					Physics2D.queriesHitTriggers = false;
					break;
				case QueryTriggerInteraction.Collide:
					Physics2D.queriesHitTriggers = true;
					break;
			}
		}

		protected virtual void EndCast()
		{
			Physics2D.queriesHitTriggers = hitTrigger;
		}

		void OnDrawGizmos()
		{
			if (!Application.isPlaying)
				Cast();
		}
	}
}