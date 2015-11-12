using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo.Internal.Physics
{
	public abstract class GravityComponentBase : PMonoBehaviour
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
		protected bool hasChanged = true;

		protected virtual void UpdateGravity()
		{
			Vector3 currentGravity = GetGravity();

			if (!hasChanged && lastGravity == currentGravity)
				return;

			gravity = rotationQuaternion * currentGravity * gravityScale;
			hasChanged = false;
			lastGravity = currentGravity;
		}

		protected abstract Vector3 GetGravity();
	}
}