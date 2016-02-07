using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo
{
	[Serializable]
	public struct Disk : IShape2D, IEquatable<Disk>
	{
		public float X;
		public float Y;
		public float InnerRadius;
		public float OuterRadius;

		public float XMin
		{
			get { return X - OuterRadius; }
		}
		public float XMax
		{
			get { return X + OuterRadius; }
		}
		public float YMin
		{
			get { return Y - OuterRadius; }
		}
		public float YMax
		{
			get { return Y + OuterRadius; }
		}
		public float InnerXMin
		{
			get { return X - InnerRadius; }
		}
		public float InnerXMax
		{
			get { return X + InnerRadius; }
		}
		public float InnerYMin
		{
			get { return Y - InnerRadius; }
		}
		public float InnerYMax
		{
			get { return Y + InnerRadius; }
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
		public Vector2 InnerTop
		{
			get { return new Vector2(X, InnerYMax); }
		}
		public Vector2 InnerBottom
		{
			get { return new Vector2(X, InnerYMin); }
		}
		public Vector2 InnerLeft
		{
			get { return new Vector2(InnerXMin, Y); }
		}
		public Vector2 InnerRight
		{
			get { return new Vector2(InnerXMax, Y); }
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
		public MinMax Radius
		{
			get { return new MinMax(InnerRadius, OuterRadius); }
			set
			{
				InnerRadius = value.Min;
				OuterRadius = value.Min;
			}
		}
		public Circle OuterCircle
		{
			get { return new Circle(X, Y, OuterRadius); }
		}
		public Circle InnerCircle
		{
			get { return new Circle(X, Y, InnerRadius); }
		}
		public Rect Bounds
		{
			get { return OuterCircle.Bounds; }
		}
		public Rect InnerBounds
		{
			get { return InnerCircle.Bounds; }
		}

		public Disk(float x, float y, float innerRadius, float outerRadius)
		{
			X = x;
			Y = y;
			InnerRadius = innerRadius;
			OuterRadius = outerRadius;
		}

		public Disk(float x, float y, MinMax radius) : this(x, y, radius.Min, radius.Max) { }

		public Disk(Vector2 position, float innerRadius, float outerRadius) : this(position.x, position.y, innerRadius, outerRadius) { }

		public Disk(Vector2 position, MinMax radius) : this(position.x, position.y, radius.Min, radius.Max) { }

		public Disk(Disk disk) : this(disk.X, disk.Y, disk.InnerRadius, disk.OuterRadius) { }

		public Vector2 GetRandomPoint()
		{
			float magnitude = PRandom.Range(InnerRadius, OuterRadius);
			var direction = Vector2.right.Rotate(PRandom.Range(0f, 360f));

			return direction * magnitude + Position;
		}

		public bool Contains(Vector2 point)
		{
			if (!Bounds.Contains(point))
				return false;

			return Vector2.Distance(Position, point).IsBetween(Mathf.Abs(InnerRadius), Mathf.Abs(OuterRadius));
		}

		public bool Contains(Rect rect)
		{
			if (!Bounds.Overlaps(rect))
				return false;

			return OuterCircle.Contains(rect) && !InnerCircle.Overlaps(rect);
		}

		public bool Contains(Circle circle)
		{
			if (!Bounds.Overlaps(circle.Bounds))
				return false;

			return Vector2.Distance(Position, circle.Position).IsBetween(InnerRadius + circle.Radius, OuterRadius - circle.Radius);
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
			return circle.Overlaps(this);
		}

		public bool Overlaps(Disk disk)
		{
			if (!Overlaps(disk.OuterCircle))
				return false;

			return !InnerCircle.Contains(disk.OuterCircle) && !disk.InnerCircle.Contains(OuterCircle);
		}

		public bool Equals(Disk other)
		{
			return
				X == other.X &&
				Y == other.Y &&
				InnerRadius == other.InnerRadius &&
				OuterRadius == other.OuterRadius;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is Disk))
				return false;

			return Equals((Disk)obj);
		}

		public override int GetHashCode()
		{
			return
				X.GetHashCode() ^
				Y.GetHashCode() ^
				InnerRadius.GetHashCode() ^
				OuterRadius.GetHashCode();
		}

		public override string ToString()
		{
			return string.Format("Circle({0}, {1}, {2}, {3})", X, Y, InnerRadius, OuterRadius);
		}
	}
}