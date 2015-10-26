using UnityEngine;
using System.Collections;

namespace Pseudo {
	public static class CameraExtensions {

		public static Vector3 GetWorldMousePosition(this Camera camera, float depth) {
			Vector3 mousePosition = Input.mousePosition;
			mousePosition.z = depth - camera.transform.position.z;
			
			return camera.ScreenToWorldPoint(mousePosition);
		}

		public static Vector3 GetWorldMousePosition(this Camera camera) {
			return GetWorldMousePosition(camera, 0);
		}
		
		public static bool WorldPointInView(this Camera camera, Vector3 worldPoint) {
			Vector3 viewPoint = camera.WorldToViewportPoint(worldPoint);
			
			return viewPoint.x >= 0 && viewPoint.x <= 1 && viewPoint.y >= 0 && viewPoint.y <= 1;
		}
	
		public static bool ScreenPointInView(this Camera camera, Vector2 screenPoint) {
			Vector3 viewPoint = camera.ScreenToViewportPoint(screenPoint);
			
			return viewPoint.x >= 0 && viewPoint.x <= 1 && viewPoint.y >= 0 && viewPoint.y <= 1;
		}
		
		public static bool WorldRectInView(this Camera camera, Rect worldRect, float depth) {
			return worldRect.Intersects(camera.GetRect(depth));
		}

		public static bool WorldRectInView(this Camera camera, Rect worldRect) {
			return camera.WorldRectInView(worldRect, 0);
		}
		
		public static Rect GetRect(this Camera camera, float depth) {
			float distance = depth - camera.transform.position.z;
			
			Vector2 min = camera.ViewportToWorldPoint(new Vector3(0, 0, distance));
			Vector2 max = camera.ViewportToWorldPoint(new Vector3(1, 1, distance));
			
			return Rect.MinMaxRect(min.x, min.y, max.x, max.y);
		}

		public static Rect GetRect(this Camera camera) {
			return GetRect(camera, 0);
		}
		
		public static Vector3 ClampToScreen(this Camera camera, Vector3 worldPoint) {
			Rect rect = camera.GetRect(worldPoint.z);
			
			worldPoint.x = Mathf.Clamp(worldPoint.x, rect.xMin, rect.xMax);
			worldPoint.y = Mathf.Clamp(worldPoint.y, rect.yMin, rect.yMax);
			
			return worldPoint;
		}
	}
}
