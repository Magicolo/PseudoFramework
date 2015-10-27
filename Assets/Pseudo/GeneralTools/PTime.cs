using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

public class PTime : PSingleton<PTime>
{
	public class TimeChannel
	{
		public float Time;
		public float DeltaTime;
		public float FixedDeltaTime;
		public float TimeScale = 1f;

		TimeChannels _channel;
		public TimeChannels Channel { get { return _channel; } }

		public TimeChannel(TimeChannels channel)
		{
			_channel = channel;
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
	readonly static List<TimeChannel> _channels = new List<TimeChannel>() { Unity, UI, World, Player, Enemy };

	void Update()
	{
		for (int i = 0; i < _channels.Count; i++)
		{
			TimeChannel timeChannel = _channels[i];
			timeChannel.DeltaTime = Time.deltaTime * timeChannel.TimeScale;
			timeChannel.Time += timeChannel.DeltaTime;
		}
	}

	void FixedUpdate()
	{
		for (int i = 0; i < _channels.Count; i++)
		{
			TimeChannel timeChannel = _channels[i];
			timeChannel.FixedDeltaTime = Time.fixedDeltaTime * timeChannel.TimeScale;
		}
	}

	public static float GetDeltaTime(TimeChannels channel)
	{
		return _channels[(int)channel].DeltaTime;
	}

	public static float GetFixedDeltaTime(TimeChannels channel)
	{
		return _channels[(int)channel].FixedDeltaTime;
	}

	public static float GetTime(TimeChannels channel)
	{
		return _channels[(int)channel].Time;
	}

	public static float GetTimeScale(TimeChannels channel)
	{
		return _channels[(int)channel].TimeScale;
	}

	public static void SetTimeScale(TimeChannels channel, float timeScale)
	{
		_channels[(int)channel].TimeScale = timeScale;
	}
}
