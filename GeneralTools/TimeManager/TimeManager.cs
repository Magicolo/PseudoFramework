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
		protected static List<TimeChannel> channels;

		protected override void Awake()
		{
			base.Awake();

			TimeChannels[] channelValues = (TimeChannels[])Enum.GetValues(typeof(TimeChannels));
			channels = new List<TimeChannel>(channelValues.Length);

			for (int i = 0; i < channelValues.Length; i++)
			{
				TimeChannels channelValue = channelValues[i];
				TimeChannel channel = CreateChannel(channelValue);
				channels.Add(channel);
			}
		}

		public static TimeChannel GetChannel(TimeChannels channel)
		{
			if (channels == null)
			{
				Debug.LogError("No instance of the TimeManager has been found.");
				return null;
			}

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
			return instance.CachedGameObject.AddChild(channel.ToString()).AddComponent<TimeChannel>();
		}
	}
}