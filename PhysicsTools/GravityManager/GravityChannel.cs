using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal.Physics;

namespace Pseudo
{
	[Serializable]
	public class GravityChannel : GravityChannelBase
	{
		protected override Vector3 GetGravity()
		{
			return GravityManager.GetChannel(channel).Gravity;
		}

		protected override Vector2 GetGravity2D()
		{
			return GravityManager.GetChannel(channel).Gravity2D;
		}
	}
}