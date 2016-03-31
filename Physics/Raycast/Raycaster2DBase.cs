using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.EntityFramework;

namespace Pseudo.Physics.Internal
{
	public abstract class Raycaster2DBase : ComponentBehaviourBase, IRaycaster
	{
		public readonly List<RaycastHit2D> Hits = new List<RaycastHit2D>();

		public LayerMask Mask = Physics2D.DefaultRaycastLayers;
		public QueryTriggerInteraction HitTrigger;
		public QueryColliderInteration HitStartCollider;
		public bool Draw = true;

		bool hitTrigger;
		bool hitStartCollider;

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

		void EndCast()
		{
			Physics2D.queriesHitTriggers = hitTrigger;
			Physics2D.queriesStartInColliders = hitStartCollider;
		}

		void OnDrawGizmos()
		{
			if (!Application.isPlaying)
				Cast();
		}

		protected abstract void UpdateCast();
	}
}