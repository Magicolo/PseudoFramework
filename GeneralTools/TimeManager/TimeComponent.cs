﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal.Entity;

namespace Pseudo
{
	public class TimeComponent : PMonoBehaviour, ITimeChannel, IComponent
	{
		public TimeManager.TimeChannels Channel
		{
			get { return time.Channel; }
		}

		public float Time
		{
			get { return time.Time; }
		}

		public float TimeScale
		{
			get { return time.TimeScale; }
			set { time.TimeScale = value; }
		}

		public float DeltaTime
		{
			get { return time.DeltaTime; }
		}

		public float FixedDeltaTime
		{
			get { return time.FixedDeltaTime; }
		}

		[SerializeField]
		TimeChannel time = null;
	}
}