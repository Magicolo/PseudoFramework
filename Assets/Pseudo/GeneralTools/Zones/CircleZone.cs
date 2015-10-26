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
		Circle _circle = new Circle(0f, 0f, 1f);
		[SerializeField]
		bool _draw = true;

		public Circle LocalCircle { get { return _circle; } set { _circle = value; } }
		public Circle WorldCircle
		{
			get
			{
				Circle circle = new Circle(_circle);
				circle.Position += transform.position.ToVector2();
				return circle;
			}
		}

		void OnDrawGizmos()
#if UNITY_EDITOR
		{
			if (!_draw)
				return;

			Vector3 position = transform.position + _circle.Position.ToVector3();
			UnityEditor.Handles.color = new Color(1f, 0f, 0f, 0.75f);
			UnityEditor.Handles.DrawWireDisc(position, Vector3.back, _circle.Radius);
			UnityEditor.Handles.color = new Color(1f, 0f, 0f, 0.1f);
			UnityEditor.Handles.DrawSolidDisc(position, Vector3.back, _circle.Radius);
#endif
		}

		public override Vector2 GetRandomLocalPoint()
		{
			return LocalCircle.GetRandomPoint();
		}

		public override Vector2 GetRandomWorldPoint()
		{
			return WorldCircle.GetRandomPoint();
		}
	}
}