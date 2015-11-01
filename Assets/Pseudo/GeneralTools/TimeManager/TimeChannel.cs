using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo.Internal
{
	public class TimeChannel : TimeComponentBase
	{
		void Awake()
		{
			channel = (TimeManager.TimeChannels)Enum.Parse(typeof(TimeManager.TimeChannels), name);
		}

		protected override float GetTime()
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
	}
}