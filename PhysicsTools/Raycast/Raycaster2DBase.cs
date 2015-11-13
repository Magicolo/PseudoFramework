using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo.Internal.Physics
{
	[ExecuteInEditMode]
	public abstract class Raycaster2DBase : PMonoBehaviour
	{
		public readonly List<RaycastHit2D> Hits = new List<RaycastHit2D>();

		public LayerMask Mask = Physics2D.DefaultRaycastLayers;
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