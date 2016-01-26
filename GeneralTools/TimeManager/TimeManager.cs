using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Internal;

namespace Pseudo
{
	public static class TimeManager
	{
		public enum TimeChannels
		{
			Unity,
			Game,
			UI,
			World,
			Player,
			Enemy,
		}

		public static readonly ITimeChannel Unity = new UnityTimeChannel();
		public static readonly ITimeChannel Game = new GlobalTimeChannel(TimeChannels.Game);
		public static readonly ITimeChannel UI = new GlobalTimeChannel(TimeChannels.UI);
		public static readonly ITimeChannel World = new GlobalTimeChannel(TimeChannels.World);
		public static readonly ITimeChannel Player = new GlobalTimeChannel(TimeChannels.Player);
		public static readonly ITimeChannel Enemy = new GlobalTimeChannel(TimeChannels.Enemy);

		static List<ITimeChannel> channels = new List<ITimeChannel>
		{
			Unity,
			Game,
			UI,
			World,
			Player,
			Enemy
		};

		public static ITimeChannel GetChannel(TimeChannels channel)
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