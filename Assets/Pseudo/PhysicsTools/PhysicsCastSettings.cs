using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo
{
	[System.Serializable]
	public class SphereCastAllSettings
	{

		public Vector3 Offset;
		public float Radius = 1;
		public LayerMask Mask;

		public bool HasHit(Vector3 origin, bool debug = false)
		{
			return GetHits(origin, debug).Length > 0;
		}

		public RaycastHit[] GetHits(Vector3 origin, bool debug = false)
		{
			RaycastHit[] hits = Physics.SphereCastAll(origin + Offset, Radius, Vector3.forward, Mathf.Infinity, Mask);

			if (debug)
				DrawRays(origin);

			return hits;
		}

		public void DrawRays(Vector3 origin)
		{
			DrawRays(origin, 16);
		}

		public void DrawRays(Vector3 origin, int amountOfRays)
		{
			for (int i = 0; i < amountOfRays; i++)
			{
				Debug.DrawRay(origin + Offset, Vector2.up.Rotate(i * (360 / amountOfRays)) * Radius, Color.yellow);
			}
		}
	}

	[System.Serializable]
	public class GroundCastSettings
	{

		public Vector3 Offset;
		[Range(-90, 90)]
		public float Spread = 30;
		[Min]
		public float Distance = 1;
		[Range(0, 360)]
		public float Angle;
		public LayerMask Mask;

		public bool HasHit(Vector3 origin, Vector3 direction, bool debug = false)
		{
			return GetHits(origin, direction, debug).Length > 0;
		}

		public Collider GetGround(Vector2 origin, Vector2 direction, bool debug = false)
		{
			RaycastHit[] hits = GetHits(origin, direction, debug);

			return hits.Length > 0 ? hits[0].collider : null;
		}

		public T GetGround<T>(Vector2 origin, Vector2 direction, bool debug = false) where T : Collider
		{
			return (T)GetGround(origin, direction, debug);
		}

		public RaycastHit[] GetHits(Vector3 origin, Vector3 direction, bool debug = false)
		{
			List<RaycastHit> hits = new List<RaycastHit>();
			float adjustedDistance = Distance / Mathf.Cos(Spread * Mathf.Deg2Rad);
			Vector3 adjustedOrigin = origin + (Angle == 0 ? Offset : Offset.Rotate(Angle));
			Vector3 adjustedDirection = Angle == 0 ? direction : direction.Rotate(Angle);

			RaycastHit hit;
			if (Physics.Raycast(adjustedOrigin, adjustedDirection, out hit, Distance, Mask))
				hits.Add(hit);

			if (Physics.Raycast(adjustedOrigin, adjustedDirection.Rotate(Spread), out hit, adjustedDistance, Mask))
				hits.Add(hit);

			if (Physics.Raycast(adjustedOrigin, adjustedDirection.Rotate(-Spread), out hit, adjustedDistance, Mask))
				hits.Add(hit);

			if (debug)
				DrawRays(origin, direction);

			return hits.ToArray();
		}

		public void DrawRays(Vector3 origin, Vector3 direction)
		{
			float adjustedDistance = Distance / Mathf.Cos(Spread * Mathf.Deg2Rad);
			Vector3 adjustedOrigin = origin + Offset.Rotate(Angle);
			Vector3 adjustedDirection = Angle == 0 ? direction : direction.Rotate(Angle);

			Debug.DrawRay(adjustedOrigin, adjustedDirection * Distance, Color.green);
			Debug.DrawRay(adjustedOrigin, adjustedDirection.Rotate(Spread) * adjustedDistance, Color.green);
			Debug.DrawRay(adjustedOrigin, adjustedDirection.Rotate(-Spread) * adjustedDistance, Color.green);
		}
	}

	[System.Serializable]
	public class GroundCastSettings2D
	{
		public Vector2 Offset;
		[Range(-90, 90)]
		public float Spread = 30;
		[Min]
		public float Distance = 1;
		[Range(0, 360)]
		public float Angle;
		public LayerMask Mask;

		public bool HasHit(Vector2 origin, Vector2 direction, bool debug = false)
		{
			return GetHits(origin, direction, debug).Length > 0;
		}

		public Collider2D GetGround(Vector2 origin, Vector2 direction, bool debug = false)
		{
			RaycastHit2D[] hits = GetHits(origin, direction, debug);

			return hits.Length > 0 ? hits[0].collider : null;
		}

		public T GetGround<T>(Vector2 origin, Vector2 direction, bool debug = false) where T : Collider2D
		{
			return (T)GetGround(origin, direction, debug);
		}

		public RaycastHit2D[] GetHits(Vector2 origin, Vector2 direction, bool debug = false)
		{
			List<RaycastHit2D> hits = new List<RaycastHit2D>();
			float adjustedDistance = Distance / Mathf.Cos(Spread * Mathf.Deg2Rad);
			Vector2 adjustedOrigin = origin + (Angle == 0 ? Offset : Offset.Rotate(Angle));
			Vector2 adjustedDirection = Angle == 0 ? direction : direction.Rotate(Angle);

			RaycastHit2D hit;

			hit = Physics2D.Raycast(adjustedOrigin, adjustedDirection, Distance, Mask, 0);
			if (hit)
				hits.Add(hit);

			hit = Physics2D.Raycast(adjustedOrigin, adjustedDirection.Rotate(Spread), adjustedDistance, Mask, 0);
			if (hit)
				hits.Add(hit);

			hit = Physics2D.Raycast(adjustedOrigin, adjustedDirection.Rotate(-Spread), adjustedDistance, Mask, 0);
			if (hit)
				hits.Add(hit);

			if (debug)
				DrawRays(origin, direction);

			return hits.ToArray();
		}

		public void DrawRays(Vector2 origin, Vector2 direction)
		{
			float adjustedDistance = Distance / Mathf.Cos(Spread * Mathf.Deg2Rad);
			Vector2 adjustedOrigin = origin + (Angle == 0 ? Offset : Offset.Rotate(Angle));
			Vector2 adjustedDirection = Angle == 0 ? direction : direction.Rotate(Angle);

			Debug.DrawRay(adjustedOrigin, adjustedDirection * Distance, Color.green);
			Debug.DrawRay(adjustedOrigin, adjustedDirection.Rotate(Spread) * adjustedDistance, Color.green);
			Debug.DrawRay(adjustedOrigin, adjustedDirection.Rotate(-Spread) * adjustedDistance, Color.green);
		}
	}
}
