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
			UI,
			World,
			Player,
			Enemy
		}

		public static TimeChannel Unity { get { return GetChannel(TimeChannels.Unity); } }
		public static TimeChannel UI { get { return GetChannel(TimeChannels.UI); } }
		public static TimeChannel World { get { return GetChannel(TimeChannels.World); } }
		public static TimeChannel Player { get { return GetChannel(TimeChannels.Player); } }
		public static TimeChannel Enemy { get { return GetChannel(TimeChannels.Enemy); } }
		static List<TimeChannel> channels = new List<TimeChannel>
		{
			CreateChannel(TimeChannels.Unity),
			CreateChannel(TimeChannels.UI),
			CreateChannel(TimeChannels.World),
			CreateChannel(TimeChannels.Player),
			CreateChannel(TimeChannels.Enemy),
		};

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

		static TimeChannel CreateChannel(TimeChannels channel)
		{
			var timeChannel = TypePoolManager.Create<TimeChannel>();
			timeChannel.Channel = channel;

			return timeChannel;
		}
	}
}