using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal;

namespace Pseudo
{
	[AddComponentMenu("Pseudo/General/Zones/Rect Zone")]
	public class RectZone : Zone2DBase
	{
		[SerializeField]
		Rect rect = new Rect(0f, 0f, 1f, 1f);
		[SerializeField]
		bool draw = true;

		public Rect LocalRect { get { return new Rect(rect.position - rect.size / 2f, rect.size); } set { rect = new Rect(value.center, value.size); } }
		public Rect WorldRect
		{
			get
			{
				Rect rect = LocalRect;
				rect.position += Transform.position.ToVector2();
				return rect;
			}
		}

		void OnDrawGizmos()
		{
			if (!draw)
				return;

			Vector3 position = Transform.position + rect.position.ToVector3();
			Vector3 size = rect.size;
			Gizmos.color = new Color(1f, 0f, 0f, 0.6f);
			Gizmos.DrawWireCube(position, size);
			Gizmos.color = new Color(1f, 0f, 0f, 0.15f);
			Gizmos.DrawCube(position, size);
		}

		public override Vector2 GetRandomLocalPoint()
		{
			return LocalRect.GetRandomPoint();
		}

		public override Vector2 GetRandomWorldPoint()
		{
			return WorldRect.GetRandomPoint();
		}
	}
}