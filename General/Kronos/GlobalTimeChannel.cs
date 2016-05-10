using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo.Internal
{
	public class GlobalTimeChannel : TimeChannelBase
	{
		public GlobalTimeChannel(Chronos.TimeChannels channel)
		{
			this.channel = channel;
		}

		protected override float GetTime()
		{
			return Chronos.Unity.Time;
		}

		protected override float GetDeltaTime()
		{
			return Chronos.Unity.DeltaTime;
		}

		protected override float GetFixedDeltaTime()
		{
			return Chronos.Unity.FixedDeltaTime;
		}
	}
}