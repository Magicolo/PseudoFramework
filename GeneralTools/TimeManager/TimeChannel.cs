using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal;
using Pseudo.Internal.Time;

namespace Pseudo
{
	[Serializable]
	public class TimeChannel : TimeChannelBase
	{
		protected override float GetTime()
		{
			return TimeManager.GetChannel(channel).Time;
		}

		protected override float GetDeltaTime()
		{
			return TimeManager.GetChannel(channel).DeltaTime;
		}

		protected override float GetFixedDeltaTime()
		{
			return TimeManager.GetChannel(channel).FixedDeltaTime;
		}
	}
}