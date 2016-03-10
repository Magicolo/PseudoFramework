using UnityEngine;
using System.Collections;

namespace Pseudo
{
	public static class RectExtensions
	{
		public static Rect Round(this Rect rect, float step)
		{
			if (step <= 0)
				return rect;

			rect.x = rect.x.Round(step);
			rect.y = rect.y.Round(step);
			rect.width = rect.width.Round(step);
			rect.height = rect.height.Round(step);

			return rect;
		}

		public static Rect Round(this Rect rect)
		{
			return rect.Round(1f);
		}

		public static Vector2 Clamp(this Rect rect, Vector2 point)
		{
			return new Vector2(Mathf.Clamp(point.x, rect.xMin, rect.xMax), Mathf.Clamp(point.y, rect.yMin, rect.yMax));
		}

		public static Rect Clamp(this Rect rect, Rect otherRect)
		{
			return new Rect
			{
				xMin = Mathf.Max(otherRect.xMin, rect.xMin),
				xMax = Mathf.Min(otherRect.xMax, rect.xMax),
				yMin = Mathf.Max(otherRect.yMin, rect.yMin),
				yMax = Mathf.Min(otherRect.yMax, rect.yMax)
			};
		}


		public static bool Contains(this Rect rect, Rect otherRect)
		{
			return
				rect.xMin < otherRect.xMin &&
				rect.xMax > otherRect.xMax &&
				rect.yMin < otherRect.yMin &&
				rect.yMax > otherRect.yMax;
		}

		public static bool Contains(this Rect rect, Circle circle)
		{
			return rect.Contains(circle.Bounds);
		}

		public static bool Contains(this Rect rect, Disk disk)
		{
			return rect.Contains(disk.Bounds);
		}

		public static bool IsContained(this Rect rect, Rect otherRect)
		{
			return otherRect.Contains(rect);
		}

		public static bool IsContained(this Rect rect, Circle circle)
		{
			return circle.Contains(rect);
		}

		public static bool IsContained(this Rect rect, Disk disk)
		{
			return disk.Contains(rect);
		}

		public static bool Overlaps(this Rect rect, Circle circle)
		{
			if (!rect.Overlaps(circle.Bounds))
				return false;

			var clamped = rect.Clamp(circle.Position);
			float distance = (circle.Position - clamped).sqrMagnitude;

			return distance < circle.Radius * circle.Radius;
		}

		public static bool Overlaps(this Rect rect, Disk disk)
		{
			return rect.Overlaps(disk.OuterCircle) && !disk.InnerCircle.Contains(rect);
		}

		public static Vector2 TopLeft(this Rect rect)
		{
			return new Vector2(rect.xMin, rect.yMin);
		}

		public static Vector2 TopRight(this Rect rect)
		{
			return new Vector2(rect.xMax, rect.yMin);
		}

		public static Vector2 BottomLeft(this Rect rect)
		{
			return new Vector2(rect.xMin, rect.yMax);
		}

		public static Vector2 BottomRight(this Rect rect)
		{
			return new Vector2(rect.xMax, rect.yMin);
		}

		public static Vector2 GetRandomPoint(this Rect rect)
		{
			return new Vector2(UnityEngine.Random.Range(rect.xMin, rect.xMax), UnityEngine.Random.Range(rect.yMin, rect.yMax));
		}
	}
}
