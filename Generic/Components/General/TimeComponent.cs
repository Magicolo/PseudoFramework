using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.EntityFramework;
using Pseudo.Pooling;

namespace Pseudo
{
	[AddComponentMenu("Pseudo/Time")]
	public class TimeComponent : PMonoBehaviour, ITimeChannel
	{
		public Chronos.TimeChannels Channel
		{
			get { return time.Channel; }
		}
		public float Time
		{
			get { return time.Time; }
		}
		public float TimeScale
		{
			get { return time.TimeScale; }
			set { time.TimeScale = value; }
		}
		public float DeltaTime
		{
			get { return time.DeltaTime; }
		}
		public float FixedDeltaTime
		{
			get { return time.FixedDeltaTime; }
		}

		[SerializeField]
		TimeChannel time = new TimeChannel();

		void OnCreate()
		{
			time.Reset();
		}

		public static implicit operator TimeChannel(TimeComponent time)
		{
			return time.time;
		}
	}
}