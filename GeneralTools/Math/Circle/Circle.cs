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
		public float X;
		public float Y;
		public float Radius;

		public float XMin { get { return X - Radius; } }
		public float XMax { get { return X + Radius; } }
		public float YMin { get { return Y - Radius; } }
		public float YMax { get { return Y + Radius; } }
		public Vector2 Position { get { return new Vector2(X, Y); } set { X = value.x; Y = value.y; } }

		public Circle(float x, float y, float radius)
		{
			X = x;
			Y = y;
			Radius = radius;
		}

		public Circle(Vector2 position, float radius)
		{
			X = position.x;
			Y = position.y;
			Radius = radius;
		}

		public Circle(Circle circle)
		{
			X = circle.X;
			Y = circle.Y;
			Radius = circle.Radius;
		}

		public Vector2 GetRandomPoint()
		{
			return UnityEngine.Random.insideUnitCircle * Radius + Position;
		}

		public bool Contains(Vector2 point)
		{
			return Vector2.Distance(Position, point) <= Mathf.Abs(Radius);
		}

		public bool Intersects(Circle circle)
		{
			return Vector2.Distance(Position, circle.Position) <= Radius + circle.Radius;
		}

		public override string ToString()
		{
			return string.Format("Circle({0}, {1}, {2})", X, Y, Radius);
		}
	}
}