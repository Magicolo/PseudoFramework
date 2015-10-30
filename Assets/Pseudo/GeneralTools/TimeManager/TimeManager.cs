using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo
{
	public class TimeManager : Singleton<TimeManager>
	{
		public enum TimeChannels
		{
			Unity,
			UI,
			World,
			Player,
			Enemy
		}

		public class TimeChannel
		{
			public TimeChannels Channel { get { return channel; } }
			public float Time { get { return time; } }
			public float TimeScale { get { return timeScale; } set { timeScale = value; } }
			public float DeltaTime { get { return deltaTime; } }
			public float FixedDeltaTime { get { return fixedDeltaTime; } }

			TimeChannels channel;
			float time;
			float timeScale = 1f;
			float deltaTime;
			float fixedDeltaTime;

			public TimeChannel(TimeChannels channel)
			{
				this.channel = channel;
			}

			public void Update()
			{
				deltaTime = UnityEngine.Time.deltaTime * timeScale;
				fixedDeltaTime = UnityEngine.Time.fixedDeltaTime * timeScale;
				time += fixedDeltaTime;
			}
		}

		public readonly static TimeChannel Unity = new TimeChannel(TimeChannels.Unity);
		public readonly static TimeChannel UI = new TimeChannel(TimeChannels.UI);
		public readonly static TimeChannel World = new TimeChannel(TimeChannels.World);
		public readonly static TimeChannel Player = new TimeChannel(TimeChannels.Player);
		public readonly static TimeChannel Enemy = new TimeChannel(TimeChannels.Enemy);
		protected readonly static List<TimeChannel> channels = new List<TimeChannel> { Unity, UI, World, Player, Enemy };

		protected virtual void FixedUpdate()
		{
			for (int i = 0; i < channels.Count; i++)
				channels[i].Update();
		}

		public static TimeChannel GetChannel(TimeChannels channel)
		{
			return channels[(int)channel];
		}

		public static float GetTime(TimeChannels channel)
		{
			return GetChannel(channel).Time;
		}

		public static float GetTimeScale(TimeChannels channel)
		{
			return GetChannel(channel).TimeScale;
		}

		public static float GetDeltaTime(TimeChannels channel)
		{
			return GetChannel(channel).DeltaTime;
		}

		public static float GetFixedDeltaTime(TimeChannels channel)
		{
			return GetChannel(channel).FixedDeltaTime;
		}

		public static void SetTimeScale(TimeChannels channel, float timeScale)
		{
			GetChannel(channel).TimeScale = timeScale;
		}
	}
}