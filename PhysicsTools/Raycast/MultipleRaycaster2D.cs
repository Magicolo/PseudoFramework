using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal.Physics;

namespace Pseudo
{
	[AddComponentMenu("Pseudo/Physics/Multiple Raycaster 2D")]
	public class MultipleRaycaster2D : Raycaster2DBase
	{
		public RaycastHitModes HitMode = RaycastHitModes.FirstOfEach;
		[Min(1)]
		public int Amount = 1;
		[Range(0f, 360f)]
		public float Spread = 30f;
		[Min]
		public float Distance = 1f;

		protected override void UpdateCast()
		{
			Hits.Clear();
			Vector3 position = Transform.position;
			Vector3 rotation = Transform.eulerAngles;
			Vector3 scale = Transform.lossyScale;
			float angleIncrement = 0f;

			if (Amount > 1)
			{
				angleIncrement = Spread / (Amount - 1);
				rotation.z -= Spread / 2f;
			}

			for (int i = 0; i < Amount; i++)
			{
				Vector2 direction = Quaternion.Euler(rotation) * Vector2.right;
				direction.Scale(scale);

				RaycastHit2D hit;

				if (Draw && Application.isEditor)
					Debug.DrawRay(position, direction * Distance, Color.green);

				switch (HitMode)
				{
					case RaycastHitModes.First:
						hit = Physics2D.Raycast(position, direction, Distance, Mask);

						if (hit.collider != null)
						{
							Hits.Add(hit);
							return;
						}
						break;
					case RaycastHitModes.FirstOfEach:
						hit = Physics2D.Raycast(position, direction, Distance, Mask);

						if (hit.collider != null)
							Hits.Add(hit);
						break;
					case RaycastHitModes.All:
						Hits.AddRange(Physics2D.RaycastAll(position, direction, Distance, Mask));
						break;
				}

				rotation.z += angleIncrement;
			}
		}
	}
}