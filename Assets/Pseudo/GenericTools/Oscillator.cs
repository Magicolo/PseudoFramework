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
		public float frequency = 1;
		public float amplitude = 1;
		public float center;
		public float offset;

		public float Oscillate()
		{
			return amplitude * (float)Math.Sin(frequency * Time.time + offset) + center;
		}

		public void OnCreate()
		{
		}

		public void OnRecycle()
		{
		}

		public void Copy(Oscillator reference)
		{
			frequency = reference.frequency;
			amplitude = reference.amplitude;
			center = reference.center;
			offset = reference.offset;
		}
	}
}