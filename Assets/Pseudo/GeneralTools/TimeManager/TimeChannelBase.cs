using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo
{
	public enum TimeChannels
	{
		Unity,
		UI,
		World,
		Player,
		Enemy
	}

	public abstract class TimeChannelBase : IPoolable, ICopyable<TimeChannelBase>
	{
		public TimeChannels Channel { get { return channel; } }
		public float TimeScale
		{
			get { return timeScale; }
			set
			{
				UpdateTime();
				timeScale = value;
			}
		}
		public float Time
		{
			get
			{
				UpdateTime();
				return time;
			}
		}
		public float DeltaTime { get { return GetDeltaTime() * timeScale; } }
		public float FixedDeltaTime { get { return GetFixedDeltaTime() * timeScale; } }

		[SerializeField]
		protected TimeChannels channel;
		[SerializeField, PropertyField]
		protected float timeScale = 1f;
		protected float time;
		protected float lastTime;

		protected virtual void UpdateTime()
		{
			float currentTime = GetCurrentTime();
			time += (currentTime - lastTime) * timeScale;
			lastTime = currentTime;
		}

		protected abstract float GetCurrentTime();
		protected abstract float GetDeltaTime();
		protected abstract float GetFixedDeltaTime();

		public virtual void OnCreate()
		{
		}

		public virtual void OnRecycle()
		{
		}

		public void Copy(TimeChannelBase reference)
		{
			channel = reference.channel;
			timeScale = reference.timeScale;
			time = reference.time;
			lastTime = reference.lastTime;
		}
	}
}