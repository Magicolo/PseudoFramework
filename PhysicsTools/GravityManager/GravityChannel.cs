using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal.Physics;

namespace Pseudo
{
	public class GravityChannel : GravityComponentBase
	{
		protected virtual void Awake()
		{
			channel = (GravityManager.GravityChannels)Enum.Parse(typeof(GravityManager.GravityChannels), name);
		}

		protected override Vector3 GetGravity()
		{
			switch (GravityManager.Mode)
			{
				default:
					return Physics2D.gravity;
				case GravityManager.Dimensions._3D:
					return Physics.gravity;
			}
		}
	}
}