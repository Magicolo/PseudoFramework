using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo
{
	public class EntityBase : PMonoBehaviour
	{
		[Serializable]
		public class TimeChannel
		{
			public TimeManager.TimeChannels Channel;
			public float TimeScale = 1f;

			public float Time { get { return TimeManager.GetTime(Channel); } }
			public float DeltaTime { get { return TimeManager.GetDeltaTime(Channel) * TimeScale; } }
			public float FixedDeltaTime { get { return TimeManager.GetFixedDeltaTime(Channel) * TimeScale; } }
		}

		public TimeChannel TimeSettings;
	}
}