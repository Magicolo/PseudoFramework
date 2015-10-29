using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo
{
	public class PTime : Singleton<PTime>
	{
		public class TimeChannel
		{
			public float Time;
			public float DeltaTime;
			public float FixedDeltaTime;
			public float TimeScale = 1f;

			TimeChannels channel;
			public TimeChannels Channel { get { return channel; } }

			public TimeChannel(TimeChannels channel)
			{
				this.channel = channel;
			}
		}

		public enum TimeChannels
		{
			Unity,
			UI,
			World,
			Player,
			Enemy,
			Count
		}

		readonly static TimeChannel Unity = new TimeChannel(TimeChannels.Unity);
		public readonly static TimeChannel UI = new TimeChannel(TimeChannels.UI);
		public readonly static TimeChannel World = new TimeChannel(TimeChannels.World);
		public readonly static TimeChannel Player = new TimeChannel(TimeChannels.Player);
		public readonly static TimeChannel Enemy = new TimeChannel(TimeChannels.Enemy);
		readonly static List<TimeChannel> channels = new List<TimeChannel> { Unity, UI, World, Player, Enemy };

		void Update()
		{
			for (int i = 0; i < channels.Count; i++)
			{
				TimeChannel timeChannel = channels[i];
				timeChannel.DeltaTime = Time.deltaTime * timeChannel.TimeScale;
				timeChannel.Time += timeChannel.DeltaTime;
			}
		}

		void FixedUpdate()
		{
			for (int i = 0; i < channels.Count; i++)
			{
				TimeChannel timeChannel = channels[i];
				timeChannel.FixedDeltaTime = Time.fixedDeltaTime * timeChannel.TimeScale;
			}
		}

		public static float GetDeltaTime(TimeChannels channel)
		{
			return channels[(int)channel].DeltaTime;
		}

		public static float GetFixedDeltaTime(TimeChannels channel)
		{
			return channels[(int)channel].FixedDeltaTime;
		}

		public static float GetTime(TimeChannels channel)
		{
			return channels[(int)channel].Time;
		}

		public static float GetTimeScale(TimeChannels channel)
		{
			return channels[(int)channel].TimeScale;
		}

		public static void SetTimeScale(TimeChannels channel, float timeScale)
		{
			channels[(int)channel].TimeScale = timeScale;
		}
	}
}