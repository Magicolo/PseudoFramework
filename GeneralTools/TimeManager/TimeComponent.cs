using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal;

namespace Pseudo
{
	[AddComponentMenu("Pseudo/General/Time"), Copy]
	public class TimeComponent : TimeComponentBase, ICopyable<TimeComponent>
	{
		protected override float GetTime()
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

		public void Copy(TimeComponent reference)
		{
			base.Copy(reference);

		}
	}
}