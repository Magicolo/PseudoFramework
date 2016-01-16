using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo.Internal.Physics
{
	public class UnityGravityChannel : IGravityChannel
	{
		public GravityManager.GravityChannels Channel
		{
			get { return GravityManager.GravityChannels.Unity; }
		}

		public Vector3 Gravity
		{
			get { return UnityEngine.Physics.gravity; }
		}

		public Vector2 Gravity2D
		{
			get { return UnityEngine.Physics2D.gravity; }
		}

		public float GravityScale
		{
			get { return gravityScale; }
			set
			{
				if (gravityScale != value)
				{
					gravityScale = value;
					UnityEngine.Physics.gravity = initialGravity * gravityScale;
					UpdateGravity();
				}
			}
		}

		public Vector3 Rotation
		{
			get { return rotation; }
			set
			{
				if (rotation != value)
				{
					rotation = value;
					rotationQuanternion = Quaternion.Euler(rotation);
					UpdateGravity();
				}
			}
		}

		float gravityScale = 1f;
		Vector3 rotation;
		Quaternion rotationQuanternion;
		Vector3 initialGravity;
		Vector2 initialGravity2D;

		public UnityGravityChannel()
		{
			initialGravity = Gravity;
			initialGravity2D = Gravity2D;
		}

		void UpdateGravity()
		{
			UnityEngine.Physics.gravity = rotationQuanternion * initialGravity * gravityScale;
			UnityEngine.Physics2D.gravity = rotationQuanternion * initialGravity2D * gravityScale;
		}
	}
}