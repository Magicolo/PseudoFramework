using UnityEngine;
using System.Collections;

namespace Pseudo
{
	public static class BoxCollider2DExtentions
	{

		public static Vector3[] GetCornersWorldPositions(this BoxCollider2D box)
		{
			Vector3[] corners = new Vector3[4];
			corners[0] = box.transform.position + new Vector3(-box.size.x / 2, -box.size.y / 2);
			corners[1] = box.transform.position + new Vector3(-box.size.x / 2, box.size.y / 2);
			corners[2] = box.transform.position + new Vector3(box.size.x / 2, -box.size.y / 2);
			corners[3] = box.transform.position + new Vector3(box.size.x / 2, box.size.y / 2);

			return corners;
		}

		/// <summary>
		/// Returns the top left corner world position of the collider
		/// </summary>
		public static Vector2 TopLeft(this BoxCollider2D collider)
		{
			float top = collider.offset.y + (collider.size.y / 2f);
			float left = collider.offset.x - (collider.size.x / 2f);
			return collider.transform.TransformPoint(new Vector2(left, top));
		}
		/// <summary>
		/// Returns the top right corner world position of the collider
		/// </summary>
		public static Vector2 TopRight(this BoxCollider2D collider)
		{
			float top = collider.offset.y + (collider.size.y / 2f);
			float right = collider.offset.x + (collider.size.x / 2f);
			return collider.transform.TransformPoint(new Vector2(right, top));
		}
		/// <summary>
		/// Returns the bottom left corner world position of the collider
		/// </summary>
		public static Vector2 BottomLeft(this BoxCollider2D collider)
		{
			float btm = collider.offset.y - (collider.size.y / 2f);
			float left = collider.offset.x - (collider.size.x / 2f);
			return collider.transform.TransformPoint(new Vector2(left, btm));
		}
		/// <summary>
		/// Returns the bottom right corner world position of the collider
		/// </summary>
		public static Vector2 BottomRight(this BoxCollider2D collider)
		{
			float btm = collider.offset.y - (collider.size.y / 2f);
			float right = collider.offset.x + (collider.size.x / 2f);
			return collider.transform.TransformPoint(new Vector2(right, btm));
		}


		public static Vector2 GetRandomPoint(this BoxCollider2D collider)
		{
			float top = collider.offset.y + (collider.size.y / 2f);
			float left = collider.offset.x - (collider.size.x / 2f);
			float btm = collider.offset.y - (collider.size.y / 2f);
			float right = collider.offset.x + (collider.size.x / 2f);
			return new Vector2(Random.Range(left, right), Random.Range(top, btm));
		}
	}

}