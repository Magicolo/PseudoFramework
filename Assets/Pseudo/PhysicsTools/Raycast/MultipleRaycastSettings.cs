using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System;
using Pseudo.Internal.Physics;

namespace Pseudo
{
	[Serializable]
	public class MultipleRaycastSettings : RaycastSettingsBase
	{
		public readonly List<RaycastHit> Hits = new List<RaycastHit>();

		[Min(2, BeforeSeparator = true)]
		public int Amount = 2;
		[Range(0f, 360f)]
		public float Angle;
		[Range(0f, 360f)]
		public float Spread = 30f;
		[Min]
		public float Distance = 1f;

		public void Cast(Vector3 origin, Vector3 forward, Vector3 upwards)
		{
			Hits.Clear();
			Vector3 position = origin + Offset;
			bool draw = Draw && Application.isEditor;
			float startAngle = Angle - Spread / 2f;
			float angleIncrement = Spread / (Amount - 1);
			float angle = startAngle;
			Vector3 cross = Vector3.Cross(forward, upwards);

			for (int i = 0; i < Amount; i++)
			{
				Vector3 direction = Quaternion.AngleAxis(angle, cross) * forward;
				RaycastHit hit;

				if (draw)
					Debug.DrawRay(position, direction * Distance, Color.green);

				switch (HitMode)
				{
					case RaycastHitModes.First:
						if (Physics.Raycast(position, direction, out hit, Distance, Mask, HitTrigger))
						{
							Hits.Add(hit);
							return;
						}
						break;
					case RaycastHitModes.FirstOfEach:
						if (Physics.Raycast(position, direction, out hit, Distance, Mask, HitTrigger))
							Hits.Add(hit);
						break;
					case RaycastHitModes.All:
						Hits.AddRange(Physics.RaycastAll(position, direction, Distance, Mask, HitTrigger));
						break;
				}

				angle += angleIncrement;
			}
		}

		public void Cast(Transform origin)
		{
			Cast(origin.position, origin.forward, origin.up);
		}
	}
}
