using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo.Internal
{
	public abstract class TimeChannelBase : ITimeChannel
	{
		public TimeManager.TimeChannels Channel
		{
			get { return channel; }
			set { channel = value; Reset(); }
		}
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

		[SerializeField, PropertyField]
		protected TimeManager.TimeChannels channel;
		[SerializeField, PropertyField]
		protected float timeScale = 1f;
		protected float time;
		protected float lastTime;

		public void Reset()
		{
			time = 0f;
			lastTime = GetTime();
		}

		protected virtual void UpdateTime()
		{
			float currentTime = GetTime();
			time += (currentTime - lastTime) * timeScale;
			lastTime = currentTime;
		}

		protected abstract float GetTime();
		protected abstract float GetDeltaTime();
		protected abstract float GetFixedDeltaTime();
	}
}