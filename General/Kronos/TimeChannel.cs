using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal;

namespace Pseudo
{
	[Serializable]
	public class TimeChannel : TimeChannelBase
	{
		protected override float GetTime()
		{
			return Chronos.GetChannel(channel).Time;
		}

		protected override float GetDeltaTime()
		{
			return Chronos.GetChannel(channel).DeltaTime;
		}

		protected override float GetFixedDeltaTime()
		{
			return Chronos.GetChannel(channel).FixedDeltaTime;
		}
	}
}