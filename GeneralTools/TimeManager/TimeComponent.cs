using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal.Entity;

namespace Pseudo
{
	public class TimeComponent : ComponentBehaviour, ITimeChannel
	{
		public TimeManager.TimeChannels Channel
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

		[SerializeField, InitializeContent]
		TimeChannel time = new TimeChannel();

		public override void OnCreate()
		{
			base.OnCreate();

			time.ResetTime();
		}

		public static implicit operator TimeChannel(TimeComponent time)
		{
			return time.time;
		}
	}
}