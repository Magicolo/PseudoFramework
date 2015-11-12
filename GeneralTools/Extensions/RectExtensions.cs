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
			return Rect.MinMaxRect(Mathf.Max(otherRect.xMin, rect.xMin), Mathf.Max(otherRect.yMin, rect.yMin), Mathf.Min(otherRect.xMax, rect.xMax), Mathf.Min(otherRect.yMax, rect.yMax));
		}

		public static bool Intersects(this Rect rect, Rect otherRect)
		{
			return !((rect.xMin > otherRect.xMax) || (rect.xMax < otherRect.xMin) || (rect.yMin > otherRect.yMax) || (rect.yMax < otherRect.yMin));
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
