using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo.Internal.Time
{
	public class GlobalTimeChannel : TimeChannelBase
	{
		public GlobalTimeChannel(TimeManager.TimeChannels channel)
		{
			this.channel = channel;
		}

		protected override float GetTime()
		{
			return TimeManager.Unity.Time;
		}

		protected override float GetDeltaTime()
		{
			return TimeManager.Unity.DeltaTime;
		}

		protected override float GetFixedDeltaTime()
		{
			return TimeManager.Unity.FixedDeltaTime;
		}
	}
}