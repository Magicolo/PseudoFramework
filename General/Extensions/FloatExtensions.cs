using System;
using UnityEngine;
using System.Collections;

namespace Pseudo
{
	public static class FloatExtensions
	{
		public static float PowSign(this float f, float power)
		{
			return Mathf.Abs(f).Pow(power) * f.Sign();
		}

		public static float Pow(this float f, float power)
		{
			if (float.IsNaN(f))
				return 0f;

			if (power == 0f)
				return 1f;

			if (power == 1f)
				return f;

			if (power == 2f)
				return f * f;

			if (power == 3f)
				return f * f * f;

			return Mathf.Pow(f, power);
		}

		public static float Round(this float f, float step)
		{
			if (float.IsNaN(f))
				return 0;

			if (step <= 0f)
				return f;

			if (step == 1f)
				return (float)Math.Round(f);

			return (float)Math.Round(f * (1d / step)) * step;
		}

		public static float Round(this float f)
		{
			return f.Round(1f);
		}

		public static bool IsBetween(this float f, float min, float max)
		{
			return f.IsBetween(min, max, false);
		}

		public static bool IsBetween(this float f, float min, float max, bool exclusive)
		{
			if (exclusive)
				return f >= min && f < max;
			else
				return f >= min && f <= max;
		}

		public static bool IsBetween(this float f, MinMax range)
		{
			return f.IsBetween(range.Min, range.Max, false);
		}

		public static bool IsBetween(this float f, MinMax range, bool exclusive)
		{
			return f.IsBetween(range.Min, range.Max, exclusive);
		}

		public static float Wrap(this float f, float min, float max)
		{
			float difference = max - min;

			while (f < min)
				f += difference;

			while (f >= max)
				f -= difference;

			return f;
		}

		public static float Wrap(this float f, MinMax range)
		{
			return f.Wrap(range.Min, range.Max);
		}

		public static float Clamp(this float f, float min, float max)
		{
			return Mathf.Clamp(f, min, max);
		}

		public static float Clamp(this float f, MinMax range)
		{
			return f.Clamp(range.Min, range.Max);
		}

		public static float Scale(this float f, float currentMin, float currentMax, float targetMin, float targetMax)
		{
			return (f - currentMin) / (currentMax - currentMin) * (targetMax - targetMin) + targetMin;
		}

		public static float Scale(this float f, MinMax currentRange, MinMax targetRange)
		{
			return f.Scale(currentRange.Min, currentRange.Max, targetRange.Min, targetRange.Max);
		}

		public static int Sign(this float f)
		{
			return f >= 0f ? 1 : -1;
		}

		public static float SetSign(this float f, bool sign)
		{
			return Mathf.Abs(f) * sign.Sign();
		}
	}
}
