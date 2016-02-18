using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal.Communication
{
	[Serializable]
	public class DelayedEvent : IEvent, IPoolable
	{
		public float Delay;
		public IEvent Event;

		int lastFrame;

		public bool Resolve()
		{
			if (Time.frameCount > lastFrame)
				Delay -= Time.deltaTime;

			if (Delay <= 0f)
				return Event.Resolve();
			else
				return false;
		}

		void IPoolable.OnCreate()
		{
			lastFrame = Time.frameCount;
		}

		void IPoolable.OnRecycle() { }
	}
}
