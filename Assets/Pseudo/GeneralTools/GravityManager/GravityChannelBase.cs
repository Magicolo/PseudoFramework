using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo
{
	public enum GravityChannels
	{
		Unity,
		World,
		Player,
		Enemy
	}

	public abstract class GravityChannelBase : IPoolable, ICopyable<GravityChannelBase>
	{
		public GravityChannels Channel { get { return channel; } }
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

		[SerializeField]
		protected GravityChannels channel;
		[SerializeField, PropertyField]
		protected float gravityScale = 1f;
		[SerializeField, PropertyField]
		protected Vector3 rotation;
		Quaternion rotationQuaternion = Quaternion.identity;
		protected Vector3 gravity;
		protected Vector2 gravity2D;
		protected Vector3 lastGravity;
		protected bool hasChanged = true;

		protected virtual void UpdateGravity()
		{
			Vector3 currentGravity = GetCurrentGravity();

			if (!hasChanged && lastGravity == currentGravity)
				return;

			gravity = rotationQuaternion * currentGravity * gravityScale;
			hasChanged = false;
			lastGravity = currentGravity;
		}

		protected abstract Vector3 GetCurrentGravity();

		public virtual void OnCreate()
		{
		}

		public virtual void OnRecycle()
		{
		}

		public void Copy(GravityChannelBase reference)
		{
			channel = reference.channel;
			gravityScale = reference.gravityScale;
			rotation = reference.rotation;
			rotationQuaternion = reference.rotationQuaternion;
			gravity = reference.gravity;
			gravity2D = reference.gravity2D;
			lastGravity = reference.lastGravity;
			hasChanged = reference.hasChanged;
		}
	}
}