using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System;

namespace Pseudo
{
	[Serializable]
	public class OscillatorOld : IPoolable, ICopyable
	{
		public float Frequency = 1;
		public float Amplitude = 1;
		public float Center;
		public float Offset;
		public TimeManager.TimeChannels TimeChannel;

		public float Oscillate()
		{
			return Amplitude * (float)Math.Sin(Frequency * TimeManager.GetTime(TimeChannel) + Offset) + Center;
		}

		public void OnCreate()
		{
		}

		public void OnRecycle()
		{
		}

		public void Copy(object reference)
		{
			var castedReference = (OscillatorOld)reference;
			Frequency = castedReference.Frequency;
			Amplitude = castedReference.Amplitude;
			Center = castedReference.Center;
			Offset = castedReference.Offset;
			TimeChannel = castedReference.TimeChannel;
		}
	}
}