using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Internal;

namespace Pseudo
{
	public class TimeManager : Singleton<TimeManager>
	{
		public readonly static GlobalTimeChannel Unity = new GlobalTimeChannel(TimeChannels.Unity);
		public readonly static GlobalTimeChannel UI = new GlobalTimeChannel(TimeChannels.UI);
		public readonly static GlobalTimeChannel World = new GlobalTimeChannel(TimeChannels.World);
		public readonly static GlobalTimeChannel Player = new GlobalTimeChannel(TimeChannels.Player);
		public readonly static GlobalTimeChannel Enemy = new GlobalTimeChannel(TimeChannels.Enemy);
		protected readonly static List<GlobalTimeChannel> channels = new List<GlobalTimeChannel> { Unity, UI, World, Player, Enemy };

		public static GlobalTimeChannel GetChannel(TimeChannels channel)
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