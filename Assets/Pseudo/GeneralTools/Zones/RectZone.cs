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
		Rect _rect = new Rect(0f, 0f, 1f, 1f);
		[SerializeField]
		bool _draw = true;

		public Rect LocalRect { get { return new Rect(_rect.position - _rect.size / 2f, _rect.size); } set { _rect = new Rect(value.center, value.size); } }
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
			if (!_draw)
				return;

			Vector3 position = Transform.position + _rect.position.ToVector3();
			Vector3 size = _rect.size;
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