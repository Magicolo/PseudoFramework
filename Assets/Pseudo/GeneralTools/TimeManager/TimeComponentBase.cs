using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo.Internal
{
	[Copy]
	public abstract class TimeComponentBase : PMonoBehaviour, ICopyable<TimeComponentBase>
	{
		public TimeManager.TimeChannels Channel { get { return channel; } }
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

		[SerializeField, Empty(DisableOnPlay = true)]
		protected TimeManager.TimeChannels channel;
		[SerializeField, PropertyField]
		protected float timeScale = 1f;
		protected float time;
		protected float lastTime;

		protected virtual void UpdateTime()
		{
			float currentTime = GetTime();
			time += (currentTime - lastTime) * timeScale;
			lastTime = currentTime;
		}

		protected abstract float GetTime();
		protected abstract float GetDeltaTime();
		protected abstract float GetFixedDeltaTime();

		public void Copy(TimeComponentBase reference)
		{
			channel = reference.channel;
			timeScale = reference.timeScale;
			time = reference.time;
			lastTime = reference.lastTime;
		}
	}
}