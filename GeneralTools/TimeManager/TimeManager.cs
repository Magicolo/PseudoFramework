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
			Enemy,
			Particle
		}

		public static TimeChannel Unity { get { return GetChannel(TimeChannels.Unity); } }
		public static TimeChannel UI { get { return GetChannel(TimeChannels.UI); } }
		public static TimeChannel World { get { return GetChannel(TimeChannels.World); } }
		public static TimeChannel Player { get { return GetChannel(TimeChannels.Player); } }
		public static TimeChannel Enemy { get { return GetChannel(TimeChannels.Enemy); } }
		public static TimeChannel Particle { get { return GetChannel(TimeChannels.Particle); } }
		protected static List<TimeChannel> channels;

		protected override void Awake()
		{
			base.Awake();

			var channelValues = (TimeChannels[])Enum.GetValues(typeof(TimeChannels));
			channels = new List<TimeChannel>(channelValues.Length);

			for (int i = 0; i < channelValues.Length; i++)
			{
				var channelValue = channelValues[i];
				var channel = CreateChannel(channelValue);
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
			if (Application.isPlaying)
				return GetChannel(channel).Time;
			else
				return 0f;
		}

		public static float GetTimeScale(TimeChannels channel)
		{
			if (Application.isPlaying)
				return GetChannel(channel).TimeScale;
			else
				return 0f;
		}

		public static float GetDeltaTime(TimeChannels channel)
		{
			if (Application.isPlaying)
				return GetChannel(channel).DeltaTime;
			else
				return 0f;
		}

		public static float GetFixedDeltaTime(TimeChannels channel)
		{
			if (Application.isPlaying)
				return GetChannel(channel).FixedDeltaTime;
			else
				return 0f;
		}

		public static void SetTimeScale(TimeChannels channel, float timeScale)
		{
			if (Application.isPlaying)
				GetChannel(channel).TimeScale = timeScale;
		}

		static TimeChannel CreateChannel(TimeChannels channel)
		{
			return instance.CachedGameObject.AddChild(channel.ToString()).AddComponent<TimeChannel>();
		}
	}
}