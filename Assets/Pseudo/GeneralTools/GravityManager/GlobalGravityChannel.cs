using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo.Internal
{
	public class GlobalGravityChannel : GravityChannelBase
	{
		protected override Vector3 GetCurrentGravity()
		{
			switch (GravityManager.Mode)
			{
				default:
					return UnityEngine.Physics2D.gravity;
				case GravityManager.Dimensions._3D:
					return UnityEngine.Physics.gravity;
			}
		}

		public GlobalGravityChannel(GravityChannels channel)
		{
			this.channel = channel;
		}
	}
}