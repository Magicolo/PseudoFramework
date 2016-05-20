using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo
{
	[AddComponentMenu("Pseudo/General/Zones/Circle Zone")]
	public class CircleZone : Zone2DBase
	{
		[SerializeField]
		Circle circle = new Circle(0f, 0f, 1f);

		public Circle LocalCircle { get { return circle; } set { circle = value; } }
		public Circle WorldCircle { get { return new Circle(circle.Position.ToVector3() + transform.position, circle.Radius); ; } }

#if UNITY_EDITOR
		[SerializeField]
		bool draw = true;
		[SerializeField]
		Color color = new Color(1f, 0f, 0f, 0.5f);

		void OnDrawGizmos()
		{
			if (!draw || !enabled || !gameObject.activeInHierarchy)
				return;

			var position = transform.position + circle.Position.ToVector3();
			UnityEditor.Handles.color = color;
			UnityEditor.Handles.DrawWireDisc(position, Vector3.back, circle.Radius);
			UnityEditor.Handles.color = color.SetValues(color.a / 4f, Channels.A);
			UnityEditor.Handles.DrawSolidDisc(position, Vector3.back, circle.Radius);
		}
#endif

		public override bool Contains(Vector3 point)
		{
			return WorldCircle.Contains(point);
		}

		public override bool Contains(Rect rect)
		{
			return rect.IsContained(WorldCircle);
		}

		public override bool Contains(IShape2D shape)
		{
			return shape.IsContained(WorldCircle);
		}

		public override bool Contains(Zone2DBase zone)
		{
			return zone.IsContained(WorldCircle);
		}

		public override bool IsContained(Rect rect)
		{
			return rect.Contains(WorldCircle);
		}

		public override bool IsContained(IShape2D shape)
		{
			return shape.Contains(WorldCircle);
		}

		public override bool IsContained(Zone2DBase zone)
		{
			return zone.Contains(WorldCircle);
		}

		public override bool Overlaps(Rect rect)
		{
			return rect.Overlaps(WorldCircle);
		}

		public override bool Overlaps(IShape2D shape)
		{
			return shape.Overlaps(WorldCircle);
		}

		public override bool Overlaps(Zone2DBase zone)
		{
			return zone.Overlaps(WorldCircle);
		}

		public override Vector2 GetRandomLocalPoint()
		{
			return LocalCircle.GetRandomPoint();
		}

		public override Vector3 GetRandomWorldPoint()
		{
			Vector3 randomPosition = WorldCircle.GetRandomPoint();
			randomPosition.z = transform.position.z;

			return randomPosition;
		}
	}
}