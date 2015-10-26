using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

public class Character3DStickSticking : State
{
	[Disable]
	public int ColliderCounter;
	[Disable]
	public Vector3 InitialGravity;
	[Disable]
	public Collider CurrentCollider;
	[Disable]
	public Collider LastCollider;

	new public Character3DStick Layer { get { return ((Character3DStick)base.Layer); } }

	public override void OnAwake()
	{
		base.OnAwake();

		InitialGravity = Layer.Gravity;
	}

	public override void OnUpdate()
	{
		base.OnUpdate();

		if (CurrentCollider == null)
		{
			Layer.Gravity.Direction = InitialGravity;
			SwitchState<Character3DStickIdle>();
		}
		else if (Layer.Jumping)
		{
			CurrentCollider = null;
			Layer.Gravity.Direction = InitialGravity;
		}
		else
			Stick();

		LastCollider = CurrentCollider;
	}

	public override void TriggerEnter(Collider collision)
	{
		base.TriggerEnter(collision);

		if (Layer.StickyLayer == collision.gameObject.layer)
		{
			ColliderCounter += 1;

			if (CurrentCollider == null)
				CurrentCollider = collision;
			else if (CurrentCollider != collision)
			{
				Vector3 direction = collision.transform.position - CachedTransform.position;
				direction = Layer.Gravity.WorldToRelative(direction) * Layer.HorizontalAxis;

				if (direction.x > 0)
					CurrentCollider = collision;
			}
		}
	}

	public override void TriggerExit(Collider collision)
	{
		base.TriggerExit(collision);

		if (Layer.StickyLayer == collision.gameObject.layer)
		{
			ColliderCounter -= 1;

			if (ColliderCounter <= 0)
				CurrentCollider = null;
		}
	}

	void Stick()
	{
		RaycastHit hit = GetHit(CurrentCollider);

		if (hit.collider != null)
			Layer.Gravity.Direction = -hit.normal;
	}

	RaycastHit GetHit(Collider colliderToHit)
	{
		Vector3 position = CachedTransform.position;
		Vector3 direction = colliderToHit.transform.position - position;
		Ray ray = new Ray(position, direction);
		RaycastHit hit;

		colliderToHit.Raycast(ray, out hit, Mathf.Infinity);

		if (Application.isEditor)
		{
			Debug.DrawRay(ray.origin, ray.direction, Color.magenta, 0.1F);
			Debug.DrawRay(hit.point, hit.normal, Color.yellow, 0.1F);
		}

		return hit;
	}
}
