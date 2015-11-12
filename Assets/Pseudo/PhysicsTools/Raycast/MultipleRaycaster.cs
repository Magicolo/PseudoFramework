using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal.Physics;

namespace Pseudo
{
	[AddComponentMenu("Pseudo/Physics/Multiple Raycaster")]
	public class MultipleRaycaster : RaycasterBase
	{
		[Min(2, BeforeSeparator = true)]
		public int Amount = 2;
		[Range(0f, 360f)]
		public float Spread = 30f;
		[Min]
		public float Distance = 1f;

		public override bool Cast()
		{
			Hits.Clear();
			Vector3 position = CachedTransform.position;
			Vector3 rotation = CachedTransform.eulerAngles;
			Vector3 scale = CachedTransform.lossyScale;
			float angleIncrement = Spread / (Amount - 1);
			rotation.z -= Spread / 2f;

			for (int i = 0; i < Amount; i++)
			{
				Vector3 direction = Quaternion.Euler(rotation) * Vector3.right;
				direction.Scale(scale);

				RaycastHit hit;

				if (Draw && Application.isEditor)
					Debug.DrawRay(position, direction * Distance, Color.green);

				switch (HitMode)
				{
					case RaycastHitModes.First:
						if (Physics.Raycast(position, direction, out hit, Distance, Mask, HitTrigger))
						{
							Hits.Add(hit);
							return true;
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

				rotation.z += angleIncrement;
			}

			return Hits.Count > 0;
		}
	}
}