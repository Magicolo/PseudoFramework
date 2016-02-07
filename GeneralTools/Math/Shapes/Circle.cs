using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo
{
	[Serializable]
	public struct Circle : IShape2D, IEquatable<Circle>
	{
		public float X;
		public float Y;
		public float Radius;

		public float XMin
		{
			get { return X - Radius; }
		}
		public float XMax
		{
			get { return X + Radius; }
		}
		public float YMin
		{
			get { return Y - Radius; }
		}
		public float YMax
		{
			get { return Y + Radius; }
		}
		public Vector2 Top
		{
			get { return new Vector2(X, YMax); }
		}
		public Vector2 Bottom
		{
			get { return new Vector2(X, YMin); }
		}
		public Vector2 Left
		{
			get { return new Vector2(XMin, Y); }
		}
		public Vector2 Right
		{
			get { return new Vector2(XMax, Y); }
		}
		public Vector2 Position
		{
			get { return new Vector2(X, Y); }
			set
			{
				X = value.x;
				Y = value.y;
			}
		}
		public Rect Bounds
		{
			get { return new Rect(X - Radius, Y - Radius, Radius * 2f, Radius * 2f); }
		}

		public Circle(float x, float y, float radius)
		{
			X = x;
			Y = y;
			Radius = radius;
		}

		public Circle(Vector2 position, float radius) : this(position.x, position.y, radius) { }

		public Circle(Circle circle) : this(circle.X, circle.Y, circle.Radius) { }

		public Vector2 GetRandomPoint()
		{
			return UnityEngine.Random.insideUnitCircle * Radius + Position;
		}

		public bool Contains(Vector2 point)
		{
			if (!Bounds.Contains(point))
				return false;

			return Vector2.Distance(Position, point) <= Mathf.Abs(Radius);
		}

		public bool Contains(Rect rect)
		{
			if (!Bounds.Contains(rect))
				return false;

			float deltaX = Mathf.Max(X - rect.xMin, rect.xMax - X);
			float deltaY = Mathf.Max(Y - rect.yMin, rect.yMax - Y);

			return deltaX * deltaX + deltaY * deltaY < Radius * Radius;
		}

		public bool Contains(Circle circle)
		{
			if (!Bounds.Contains(circle.Bounds))
				return false;

			return Vector2.Distance(Position, circle.Position) <= Radius - circle.Radius;
		}

		public bool Contains(Disk disk)
		{
			return Contains(disk.OuterCircle);
		}

		public bool IsContained(Rect rect)
		{
			return rect.Contains(this);
		}

		public bool IsContained(Circle circle)
		{
			return circle.Contains(this);
		}

		public bool IsContained(Disk disk)
		{
			return disk.Contains(this);
		}

		public bool Overlaps(Rect rect)
		{
			return rect.Overlaps(this);
		}

		public bool Overlaps(Circle circle)
		{
			if (!Bounds.Overlaps(circle.Bounds))
				return false;

			return Vector2.Distance(Position, circle.Position) <= Radius + circle.Radius;
		}

		public bool Overlaps(Disk disk)
		{
			if (!Overlaps(disk.OuterCircle))
				return false;

			return !disk.InnerCircle.Contains(this);
		}

		public bool Equals(Circle other)
		{
			return
				X == other.X &&
				Y == other.Y &&
				Radius == other.Radius;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is Circle))
				return false;

			return Equals((Circle)obj);
		}

		public override int GetHashCode()
		{
			return
				X.GetHashCode() ^
				Y.GetHashCode() ^
				Radius.GetHashCode();
		}

		public override string ToString()
		{
			return string.Format("Circle({0}, {1}, {2})", X, Y, Radius);
		}
	}
}