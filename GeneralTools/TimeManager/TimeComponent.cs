using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal.Entity3;

namespace Pseudo
{
	public class TimeComponent : PMonoBehaviour, ITimeComponent
	{
		public TimeManager.TimeChannels Channel
		{
			get { return time.Channel; }
		}

		float ITimeChannel.Time
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
		TimeChannel time;
	}

	public interface ITimeComponent : ITimeChannel, IComponent { }
}