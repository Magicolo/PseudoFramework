using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Physics.Internal;

namespace Pseudo.Physics
{
	[AddComponentMenu("Pseudo/Physics/Multiple Raycaster")]
	public class MultipleRaycaster : RaycasterBase
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
			var position = CachedTransform.position;
			var rotation = CachedTransform.eulerAngles;
			var scale = CachedTransform.lossyScale;
			float angleIncrement = 0f;

			if (Amount > 1)
			{
				angleIncrement = Spread / (Amount - 1);
				rotation.z -= Spread / 2f;
			}

			for (int i = 0; i < Amount; i++)
			{
				var direction = Quaternion.Euler(rotation) * Vector3.right;
				direction.Scale(scale);

				RaycastHit hit;

				if (Draw && Application.isEditor)
					Debug.DrawRay(position, direction * Distance, Color.green);

				switch (HitMode)
				{
					case RaycastHitModes.First:
						if (UnityEngine.Physics.Raycast(position, direction, out hit, Distance, Mask, HitTrigger))
						{
							Hits.Add(hit);
							return;
						}
						break;
					case RaycastHitModes.FirstOfEach:
						if (UnityEngine.Physics.Raycast(position, direction, out hit, Distance, Mask, HitTrigger))
							Hits.Add(hit);
						break;
					case RaycastHitModes.All:
						Hits.AddRange(UnityEngine.Physics.RaycastAll(position, direction, Distance, Mask, HitTrigger));
						break;
				}

				rotation.z += angleIncrement;
			}
		}
	}
}