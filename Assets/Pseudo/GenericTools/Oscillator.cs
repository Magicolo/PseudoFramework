using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System;

namespace Pseudo
{
	[Serializable]
	public class Oscillator : IPoolable, ICopyable<Oscillator>
	{
		public float Frequency = 1;
		public float Amplitude = 1;
		public float Center;
		public float Offset;
		public PTime.TimeChannels TimeChannel;

		public float Oscillate()
		{
			return Amplitude * (float)Math.Sin(Frequency * PTime.GetTime(TimeChannel) + Offset) + Center;
		}

		public void OnCreate()
		{
		}

		public void OnRecycle()
		{
		}

		public void Copy(Oscillator reference)
		{
			Frequency = reference.Frequency;
			Amplitude = reference.Amplitude;
			Center = reference.Center;
			Offset = reference.Offset;
		}
	}
}