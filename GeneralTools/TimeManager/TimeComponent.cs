using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal.Entity;

namespace Pseudo
{
	public class TimeComponent : ComponentBehaviour, ITimeChannel, ICopyable<TimeComponent>
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

		[SerializeField]
		TimeChannel time = new TimeChannel();

		public override void OnCreate()
		{
			base.OnCreate();

			time.ResetTime();
		}

		public void Copy(TimeComponent reference)
		{
			time.Copy(reference.time);
		}

		public static implicit operator TimeChannel(TimeComponent time)
		{
			return time.time;
		}
	}
}