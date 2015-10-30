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
		protected override float GetCurrentTime()
		{
			return UnityEngine.Time.time;
		}

		protected override float GetDeltaTime()
		{
			return UnityEngine.Time.deltaTime;
		}

		protected override float GetFixedDeltaTime()
		{
			return UnityEngine.Time.fixedDeltaTime;
		}

		public GlobalTimeChannel(TimeChannels channel)
		{
			this.channel = channel;
		}
	}
}