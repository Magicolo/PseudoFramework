using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo.Internal
{
	public class UnityTimeChannel : ITimeChannel
	{
		public TimeManager.TimeChannels Channel
		{
			get { return TimeManager.TimeChannels.Unity; }
		}

		public float DeltaTime
		{
			get { return UnityEngine.Time.deltaTime; }
		}

		public float FixedDeltaTime
		{
			get { return UnityEngine.Time.fixedDeltaTime; }
		}

		public float Time
		{
			get { return UnityEngine.Time.time; }
		}

		public float TimeScale
		{
			get { return UnityEngine.Time.timeScale; }
			set { UnityEngine.Time.timeScale = value; }
		}
	}
}