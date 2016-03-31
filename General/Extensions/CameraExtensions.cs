using UnityEngine;
using System.Collections;

namespace Pseudo
{
	public static class CameraExtensions
	{
		public static Vector3 GetMouseWorldPosition(this Camera camera, float depth = 0f)
		{
			Vector3 mousePosition = UnityEngine.Input.mousePosition;
			mousePosition.z = depth - camera.transform.position.z;

			return camera.ScreenToWorldPoint(mousePosition);
		}

		public static bool WorldPointInView(this Camera camera, Vector3 worldPoint)
		{
			Vector3 viewPoint = camera.WorldToViewportPoint(worldPoint);

			return viewPoint.x >= 0f && viewPoint.x <= 1f && viewPoint.y >= 0f && viewPoint.y <= 1f;
		}

		public static bool ScreenPointInView(this Camera camera, Vector2 screenPoint)
		{
			Vector3 viewPoint = camera.ScreenToViewportPoint(screenPoint);

			return viewPoint.x >= 0f && viewPoint.x <= 1f && viewPoint.y >= 0f && viewPoint.y <= 1f;
		}

		public static bool WorldRectInView(this Camera camera, Rect worldRect, float depth = 0f)
		{
			return worldRect.Overlaps(camera.GetWorldRect(depth));
		}

		public static Rect GetWorldRect(this Camera camera, float depth = 0f)
		{
			float distance = depth - camera.transform.position.z;

			Vector2 min = camera.ViewportToWorldPoint(new Vector3(0f, 0f, distance));
			Vector2 max = camera.ViewportToWorldPoint(new Vector3(1f, 1f, distance));

			return Rect.MinMaxRect(min.x, min.y, max.x, max.y);
		}

		public static Vector3 ClampToScreen(this Camera camera, Vector3 worldPoint, Vector2 margin = default(Vector2))
		{
			Rect rect = camera.GetWorldRect(worldPoint.z);

			worldPoint.x = Mathf.Clamp(worldPoint.x, rect.xMin + margin.x, rect.xMax - margin.x);
			worldPoint.y = Mathf.Clamp(worldPoint.y, rect.yMin + margin.y, rect.yMax - margin.y);

			return worldPoint;
		}
	}
}
