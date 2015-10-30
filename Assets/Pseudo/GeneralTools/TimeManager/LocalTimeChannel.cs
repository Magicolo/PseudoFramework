using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo
{
	[Serializable]
	public class LocalTimeChannel : TimeChannelBase, ICopyable<LocalTimeChannel>
	{
		protected override float GetCurrentTime()
		{
			return TimeManager.GetTime(channel);
		}

		protected override float GetDeltaTime()
		{
			return TimeManager.GetDeltaTime(channel);
		}

		protected override float GetFixedDeltaTime()
		{
			return TimeManager.GetFixedDeltaTime(channel);
		}

		public void Copy(LocalTimeChannel reference)
		{
			base.Copy(reference);

		}
	}
}