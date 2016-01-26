using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal.Physics;

namespace Pseudo
{
	public class GlobalGravityChannel : GravityChannelBase
	{
		public GlobalGravityChannel(GravityManager.GravityChannels channel)
		{
			this.channel = channel;
		}

		protected override Vector3 GetGravity()
		{
			return GravityManager.Unity.Gravity;
		}

		protected override Vector2 GetGravity2D()
		{
			return GravityManager.Unity.Gravity2D;
		}
	}
}