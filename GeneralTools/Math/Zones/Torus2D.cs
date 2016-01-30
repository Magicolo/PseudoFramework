using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo
{
	[AddComponentMenu("Pseudo/General/Zones/Torus2D Zone")]
	public class Torus2D : Zone2DBase
	{
		[SerializeField]
		Circle circle = new Circle(0f, 0f, 1f);
		[SerializeField]
		Circle innerCircle = new Circle(0f, 0f, 1f);
		[SerializeField]
		bool draw = true;

		public Circle LocalCircle { get { return circle; } set { circle = value; } }
		public Circle WorldCircle { get { return new Circle(circle.Position.ToVector3() + CachedTransform.position, circle.Radius); } }
		public Circle WorldInsideCircle { get { return new Circle(innerCircle.Position.ToVector3() + CachedTransform.position, innerCircle.Radius); } }

		void OnDrawGizmos()
		{
			if (!draw || !enabled || !gameObject.activeInHierarchy)
				return;

#if UNITY_EDITOR
			var position = CachedTransform.position + circle.Position.ToVector3();
			UnityEditor.Handles.color = new Color(1f, 0f, 0f, 0.75f);
			UnityEditor.Handles.DrawWireDisc(position, Vector3.back, circle.Radius);
			UnityEditor.Handles.color = new Color(1f, 0f, 0f, 0.1f);
			UnityEditor.Handles.DrawSolidDisc(position, Vector3.back, circle.Radius);
			UnityEditor.Handles.color = new Color(1f, 0f, 0f, 0.75f);
			UnityEditor.Handles.DrawWireDisc(position, Vector3.back, innerCircle.Radius);
#endif
		}

		public override Vector2 GetRandomLocalPoint()
		{
			return LocalCircle.GetRandomPoint();
		}

		public override Vector2 GetRandomWorldPoint()
		{
			if (innerCircle.Radius < circle.Radius)
			{
				while (true)
				{
					Vector3 pos = WorldCircle.GetRandomPoint();
					if (!WorldInsideCircle.Contains(pos))
						return pos;
				}
			}
			else
				return Vector2.zero;


		}
	}
}