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

		public Rect LocalRect { get { return new Rect(rect.position - rect.size / 2f, rect.size); } set { rect = new Rect(value.center, value.size); } }
		public Rect WorldRect
		{
			get
			{
				var rect = LocalRect;
				rect.position += CachedTransform.position.ToVector2();
				return rect;
			}
		}

#if UNITY_EDITOR
		[SerializeField]
		bool draw = true;
		[SerializeField]
		Color color = new Color(1f, 0f, 0f, 0.5f);

		void OnDrawGizmos()
		{
			if (!draw || !enabled || !gameObject.activeInHierarchy)
				return;

			var position = CachedTransform.position + rect.position.ToVector3();
			Vector3 size = rect.size;
			Gizmos.color = color;
			Gizmos.DrawWireCube(position, size);
			Gizmos.color = color.SetValues(color.a / 4f, Channels.A);
			Gizmos.DrawCube(position, size);
		}
#endif

		public override bool Contains(Vector3 point)
		{
			return WorldRect.Contains(point);
		}

		public override bool Contains(Rect rect)
		{
			return rect.IsContained(WorldRect);
		}

		public override bool Contains(IShape2D shape)
		{
			return shape.IsContained(WorldRect);
		}

		public override bool Contains(Zone2DBase zone)
		{
			return zone.IsContained(WorldRect);
		}

		public override bool IsContained(Rect rect)
		{
			return rect.Contains(WorldRect);
		}

		public override bool IsContained(IShape2D shape)
		{
			return shape.Contains(WorldRect);
		}

		public override bool IsContained(Zone2DBase zone)
		{
			return zone.Contains(WorldRect);
		}

		public override bool Overlaps(Rect rect)
		{
			return rect.Overlaps(WorldRect);
		}

		public override bool Overlaps(IShape2D shape)
		{
			return shape.Overlaps(WorldRect);
		}

		public override bool Overlaps(Zone2DBase zone)
		{
			return zone.Overlaps(WorldRect);
		}

		public override Vector2 GetRandomLocalPoint()
		{
			return LocalRect.GetRandomPoint();
		}

		public override Vector3 GetRandomWorldPoint()
		{
			Vector3 randomPosition = WorldRect.GetRandomPoint();
			randomPosition.z = CachedTransform.position.z;

			return randomPosition;
		}
	}
}