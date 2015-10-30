using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal.Physics
{
	public abstract class GravityBase : PMonoBehaviour
	{
		[SerializeField, PropertyField]
		protected GravityChannels gravityChannel;
		public TimeChannels TimeChannel;
		[SerializeField, PropertyField]
		protected Vector3 gravityScale = new Vector3(1f, 1f, 1f);
		[SerializeField, PropertyField]
		protected Vector3 rotation;
		protected Quaternion rotationQuaternion;
		protected Vector3 gravity;
		protected Vector3 lastGravity;

		public GravityChannels GravityChannel
		{
			get { return gravityChannel; }
			set
			{
				gravityChannel = value;
				UpdateGravityForce();
			}
		}
		public Vector3 GravityScale
		{
			get { return gravityScale; }
			set
			{
				gravityScale = value;
				UpdateGravityForce();
			}
		}
		public Vector3 Rotation
		{
			get { return rotation; }
			set
			{
				rotation = value;
				rotationQuaternion.eulerAngles = rotation;
				UpdateGravityForce();
			}
		}
		public Vector3 Gravity { get { return gravity; } }

		protected virtual void Awake()
		{
			rotationQuaternion.eulerAngles = rotation;
		}

		protected virtual void UpdateGravityForceIfNeeded()
		{
			Vector3 currentGravity = GravityManager.GetGravity(gravityChannel);

			if (lastGravity == currentGravity)
				return;

			lastGravity = currentGravity;
			UpdateGravityForce();
		}

		protected virtual void UpdateGravityForce()
		{
			gravity = rotationQuaternion * lastGravity.Mult(gravityScale);
		}

		public static implicit operator Vector2(GravityBase gravity)
		{
			return gravity.gravity;
		}

		public static implicit operator Vector3(GravityBase gravity)
		{
			return gravity.gravity;
		}
	}
}