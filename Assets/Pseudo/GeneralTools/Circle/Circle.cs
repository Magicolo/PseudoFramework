using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo
{
	[Serializable]
	public struct Circle
	{
		[SerializeField]
		float x;
		[SerializeField]
		float y;
		[SerializeField]
		float radius;

		public float XMin { get { return x - radius; } }
		public float XMax { get { return x + radius; } }
		public float YMin { get { return y - radius; } }
		public float YMax { get { return y + radius; } }
		public float Radius { get { return radius; } set { radius = value; } }
		public Vector2 Position { get { return new Vector2(x, y); } set { x = value.x; y = value.y; } }

		public Circle(float x, float y, float radius)
		{
			this.x = x;
			this.y = y;
			this.radius = radius;
		}

		public Circle(Vector2 position, float radius)
		{
			x = position.x;
			y = position.y;
			this.radius = radius;
		}

		public Circle(Circle circle)
		{
			x = circle.x;
			y = circle.y;
			radius = circle.radius;
		}

		public Vector2 GetRandomPoint()
		{
			return UnityEngine.Random.insideUnitCircle * radius + Position;
		}

		public bool Contains(Vector2 point)
		{
			return Vector2.Distance(Position, point) <= Mathf.Abs(radius);
		}

		public bool Intersects(Circle circle)
		{
			return Vector2.Distance(Position, circle.Position) <= radius + circle.radius;
		}

		public override string ToString()
		{
			return string.Format("Circle({0}, {1}, {2})", x, y, radius);
		}
	}
}