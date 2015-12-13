using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo.Internal.Physics
{
	public abstract class Raycaster2DBase : PComponent
	{
		public readonly List<RaycastHit2D> Hits = new List<RaycastHit2D>();

		public LayerMask Mask = Physics2D.DefaultRaycastLayers;
		public QueryTriggerInteraction HitTrigger = QueryTriggerInteraction.UseGlobal;
		public QueryColliderInteration HitStartCollider = QueryColliderInteration.UseGlobal;
		public bool Draw = true;

		bool hitTrigger;
		bool hitStartCollider;

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

			hitStartCollider = Physics2D.queriesStartInColliders;

			switch (HitStartCollider)
			{
				case QueryColliderInteration.Ignore:
					Physics2D.queriesStartInColliders = false;
					break;
				case QueryColliderInteration.Collide:
					Physics2D.queriesStartInColliders = true;
					break;
			}
		}

		protected virtual void EndCast()
		{
			Physics2D.queriesHitTriggers = hitTrigger;
			Physics2D.queriesStartInColliders = hitStartCollider;
		}

		void OnDrawGizmos()
		{
			if (!Application.isPlaying)
				Cast();
		}
	}
}