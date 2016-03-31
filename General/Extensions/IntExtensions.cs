using System;
using UnityEngine;

namespace Pseudo
{
	public static class IntExtensions
	{
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

		public static bool IsBetween(this int i, int min, int max)
		{
			return i.IsBetween(min, max, false);
		}

		public static bool IsBetween(this int i, int min, int max, bool exclusive)
		{
			if (exclusive)
				return i >= min && i < max;
			else
				return i >= min && i <= max;
		}

		public static bool IsBetween(this int i, MinMax range)
		{
			return i.IsBetween((int)range.Min, (int)range.Max, false);
		}

		public static bool IsBetween(this int i, MinMax range, bool exclusive)
		{
			return i.IsBetween((int)range.Min, (int)range.Max, exclusive);
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

		public static int Wrap(this int i, MinMax range)
		{
			return i.Wrap((int)range.Min, (int)range.Max);
		}

		public static int Clamp(this int i, int min, int max)
		{
			return Mathf.Clamp(i, min, max);
		}

		public static int Clamp(this int i, MinMax range)
		{
			return i.Clamp((int)range.Min, (int)range.Max);
		}

		public static int Scale(this int i, int currentMin, int currentMax, int targetMin, int targetMax)
		{
			return (i - currentMin) / (currentMax - currentMin) * (targetMax - targetMin) + targetMin;
		}

		public static int Scale(this int i, MinMax currentRange, MinMax targetRange)
		{
			return i.Scale((int)currentRange.Min, (int)currentRange.Max, (int)targetRange.Min, (int)targetRange.Max);
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
