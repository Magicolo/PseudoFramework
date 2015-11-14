using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo
{
	[AddComponentMenu("Pseudo/General/Zones/Circle Zone"), Copy]
	public class CircleZone : Zone2DBase, ICopyable<CircleZone>
	{
		[SerializeField]
		Circle circle = new Circle(0f, 0f, 1f);
		[SerializeField]
		bool draw = true;

		public Circle LocalCircle { get { return circle; } set { circle = value; } }
		public Circle WorldCircle { get { return new Circle(circle.Position.ToVector3() + CachedTransform.position, circle.Radius); ; } }

		void OnDrawGizmos()
		{
			if (!draw)
				return;

			Vector3 position = CachedTransform.position + circle.Position.ToVector3();
			UnityEditor.Handles.color = new Color(1f, 0f, 0f, 0.75f);
			UnityEditor.Handles.DrawWireDisc(position, Vector3.back, circle.Radius);
			UnityEditor.Handles.color = new Color(1f, 0f, 0f, 0.1f);
			UnityEditor.Handles.DrawSolidDisc(position, Vector3.back, circle.Radius);
		}

		public override Vector2 GetRandomLocalPoint()
		{
			return LocalCircle.GetRandomPoint();
		}

		public override Vector2 GetRandomWorldPoint()
		{
			return WorldCircle.GetRandomPoint();
		}

		public void Copy(CircleZone reference)
		{
			circle = reference.circle;
			draw = reference.draw;
		}
	}
}