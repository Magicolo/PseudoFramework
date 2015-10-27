using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System;

namespace Pseudo
{
	[Serializable]
	public class RaycastSettings2DBase
	{
		public Vector2 Offset;
		public RaycastHitModes HitMode = RaycastHitModes.FirstOfEach;
		public LayerMask Mask;
		public bool Draw = true;
	}

	[Serializable]
	public class MultipleRaycast2DSettings : RaycastSettings2DBase
	{
		public readonly List<RaycastHit2D> Hits = new List<RaycastHit2D>();

		[Min(2, BeforeSeparator = true)]
		public int Amount = 2;
		[Range(0f, 360f)]
		public float Angle;
		[Range(0f, 360f)]
		public float Spread = 30f;
		[Min]
		public float Distance = 1f;

		public void Cast(Vector2 origin, float angleOffset = 0f)
		{
			Hits.Clear();
			Vector3 position = origin + Offset;
			bool draw = Draw && Application.isEditor;
			float startAngle = Angle - Spread / 2f + angleOffset;
			float angleIncrement = Spread / (Amount - 1);
			float angle = startAngle;

			for (int i = 0; i < Amount; i++)
			{
				Vector2 direction = Quaternion.AngleAxis(angle, Vector3.forward) * Vector2.right;
				RaycastHit2D hit;

				if (draw)
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

				angle += angleIncrement;
			}
		}

		public void Cast(Transform origin)
		{
			Cast(origin.position, origin.eulerAngles.z);
		}
	}
}
