using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo
{
	[Serializable]
	public struct Point2
	{
		public int X;
		public int Y;

		public static Point2 Zero
		{
			get { return new Point2(0, 0); }
		}

		public static Point2 One
		{
			get { return new Point2(1, 1); }
		}

		public static Point2 Up
		{
			get { return new Point2(0, 1); }
		}

		public static Point2 Down
		{
			get { return new Point2(0, -1); }
		}

		public static Point2 Left
		{
			get { return new Point2(-1, 0); }
		}

		public static Point2 Right
		{
			get { return new Point2(1, 0); }
		}

		public float Magnitude
		{
			get { return Mathf.Sqrt(X * X + Y * Y); }
		}

		public float SqrMagnitude
		{
			get { return X * X + Y * Y; }
		}

		public Point2(int x, int y)
		{
			X = x;
			Y = y;
		}

		public Point2(Vector2 point) : this()
		{
			X = (int)point.x;
			Y = (int)point.y;
		}

		public override int GetHashCode()
		{
			return X.GetHashCode() ^ Y.GetHashCode() << 2;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is Point2))
			{
				return false;
			}

			var point = (Point2)obj;

			return X.Equals(point.X) && Y.Equals(point.Y);
		}

		public static Point2 operator +(Point2 a, Point2 b)
		{
			return new Point2(a.X + b.X, a.Y + b.Y);
		}

		public static Point2 operator -(Point2 a, Point2 b)
		{
			return new Point2(a.X - b.X, a.Y - b.Y);
		}

		public static Point2 operator -(Point2 a)
		{
			return new Point2(-a.X, -a.Y);
		}

		public static Point2 operator *(Point2 a, Point2 b)
		{
			return new Point2(a.X * b.X, a.Y * b.Y);
		}

		public static Point2 operator *(Point2 a, float d)
		{
			return new Point2(Mathf.RoundToInt(a.X * d), Mathf.RoundToInt(a.Y * d));
		}

		public static Point2 operator *(float d, Point2 a)
		{
			return new Point2(Mathf.RoundToInt(a.X * d), Mathf.RoundToInt(a.Y * d));
		}

		public static Point2 operator *(Point2 a, int d)
		{
			return new Point2(a.X * d, a.Y * d);
		}

		public static Point2 operator *(int d, Point2 a)
		{
			return new Point2(a.X * d, a.Y * d);
		}

		public static Point2 operator /(Point2 a, Point2 b)
		{
			return new Point2(a.X / b.X, a.Y / b.Y);
		}

		public static Point2 operator /(Point2 a, float d)
		{
			return new Point2(Mathf.RoundToInt(a.X / d), Mathf.RoundToInt(a.Y / d));
		}

		public static Point2 operator /(Point2 a, int d)
		{
			return new Point2(a.X / d, a.Y / d);
		}

		public static bool operator ==(Point2 lhs, Point2 rhs)
		{
			return lhs.X == rhs.X && lhs.Y == rhs.Y;
		}

		public static bool operator !=(Point2 lhs, Point2 rhs)
		{
			return lhs.X != rhs.X || lhs.Y != rhs.Y;
		}

		public static implicit operator Vector2(Point2 p)
		{
			return new Vector2(p.X, p.Y);
		}

		public static implicit operator Vector3(Point2 p)
		{
			return new Vector3(p.X, p.Y, 0f);
		}

		public static implicit operator Point2(Vector2 v)
		{
			return new Point2(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y));
		}

		public static implicit operator Point2(Vector3 v)
		{
			return new Point2(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y));
		}

		public override string ToString()
		{
			return "Point2(" + X + " , " + Y + ")";
		}
	}
}