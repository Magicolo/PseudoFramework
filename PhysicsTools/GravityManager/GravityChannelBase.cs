using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo.Internal.Physics
{
	public abstract class GravityChannelBase : IGravityChannel
	{
		public GravityManager.GravityChannels Channel { get { return channel; } }
		public Vector3 Gravity
		{
			get
			{
				UpdateGravity();
				return gravity;
			}
		}
		public Vector2 Gravity2D
		{
			get
			{
				UpdateGravity2D();
				return gravity2D;
			}
		}
		public float GravityScale
		{
			get { return gravityScale; }
			set
			{
				gravityScale = value;
				hasChanged = true;
			}
		}
		public Vector3 Rotation
		{
			get { return rotation; }
			set
			{
				rotation = value;
				rotationQuaternion.eulerAngles = rotation;
				hasChanged = true;
			}
		}

		[SerializeField, Empty(DisableOnPlay = true)]
		protected GravityManager.GravityChannels channel;
		[SerializeField, PropertyField]
		protected float gravityScale = 1f;
		[SerializeField, PropertyField]
		protected Vector3 rotation;
		protected Quaternion rotationQuaternion = Quaternion.identity;
		protected Vector3 gravity;
		protected Vector3 lastGravity;
		protected Vector2 gravity2D;
		protected Vector2 lastGravity2D;
		protected bool hasChanged = true;

		protected virtual void UpdateGravity()
		{
			var currentGravity = GetGravity();

			if (!hasChanged && lastGravity == currentGravity)
				return;

			gravity = rotationQuaternion * currentGravity * gravityScale;
			hasChanged = false;
			lastGravity = currentGravity;
		}

		protected virtual void UpdateGravity2D()
		{
			var currentGravity = GetGravity2D();

			if (!hasChanged && lastGravity2D == currentGravity)
				return;

			gravity2D = rotationQuaternion * currentGravity * gravityScale;
			hasChanged = false;
			lastGravity2D = currentGravity;
		}

		protected abstract Vector3 GetGravity();
		protected abstract Vector2 GetGravity2D();
	}
}