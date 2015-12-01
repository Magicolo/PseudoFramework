using System;
using UnityEngine;

namespace Pseudo
{
	public static class IntExtensions
	{
		public static int Scale(this int i, int currentMin, int currentMax, int targetMin, int targetMax)
		{
			return (i - currentMin) / (currentMax - currentMin) * (targetMax - targetMin) + targetMin;
		}

		public static float PowSign(this int i, int power)
		{
			return Mathf.Abs(i).Pow(power) * i.Sign();
		}

		public static float Pow(this int i, int power)
		{
			if (power == 0)
				return 1;

			if (power == 1)
				return i;

			if (power == 2)
				return i * i;

			if (power == 3)
				return i * i * i;

			return Mathf.Pow(i, power);
		}

		public static int Round(this int i, int step)
		{
			if (step <= 0)
				return i;

			return (int)(Math.Round(i * (1d / step)) * step);
		}

		public static int Wrap(this int i, int min, int max)
		{
			int difference = max - min;

			while (i < min)
				i += difference;

			while (i >= max)
				i -= difference;

			return i;
		}

		public static int Sign(this int i)
		{
			return i >= 0 ? 1 : -1;
		}

		public static int SetSign(this int i, bool sign)
		{
			return Mathf.Abs(i) * sign.Sign();
		}
	}
}
