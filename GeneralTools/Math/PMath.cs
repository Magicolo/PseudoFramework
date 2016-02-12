using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo
{
	public static class PMath
	{
		public static double Clamp(double value, double min, double max)
		{
			return Math.Min(Math.Max(value, min), max);
		}

		public static float MidiToFrequency(float note)
		{
			return Mathf.Pow(2, (note - 69) / 12) * 440;
		}

		public static float Hypotenuse(float a)
		{
			return Hypotenuse(a, a);
		}

		public static float Hypotenuse(float a, float b)
		{
			return Mathf.Sqrt(Mathf.Pow(a, 2f) + Mathf.Pow(b, 2f));
		}

		public static float Triangle(float phase, float ratio = 0.5f)
		{
			phase = phase % 1f;

			if (phase < ratio)
				return (phase / ratio) * 2f - 1f;
			else
				return (1f - ((phase - ratio) / (1f - ratio))) * 2f - 1f;
		}

		public static float Square(float phase, float ratio = 0.5f)
		{
			phase %= 1f;

			if (phase < ratio)
				return -1f;
			else
				return 1f;
		}
	}
}