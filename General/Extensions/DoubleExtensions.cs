using UnityEngine;
using System;
using System.Collections;

namespace Pseudo
{
	public static class DoubleExtensions
	{
		public static double PowSign(this double d, double power)
		{
			return Math.Abs(d).Pow(power) * d.Sign();
		}

		public static double Pow(this double d, double power)
		{
			if (double.IsNaN(d))
				return 0;

			if (power == 0d)
				return 1;

			if (power == 1d)
				return d;

			if (power == 2d)
				return d * d;

			if (power == 3d)
				return d * d * d;

			return Math.Pow(d, power);
		}

		public static double Round(this double d, double step)
		{
			if (double.IsNaN(d))
				return 0d;

			if (step <= 0)
				return d;

			if (step == 1d)
				return Math.Round(d);

			return Math.Round(d * (1d / step)) * step;
		}

		public static double Round(this double d)
		{
			return d.Round(1d);
		}

		public static bool IsBetween(this double d, double min, double max)
		{
			return d.IsBetween(min, max, false);
		}

		public static bool IsBetween(this double d, double min, double max, bool exclusive)
		{
			if (exclusive)
				return d >= min && d < max;
			else
				return d >= min && d <= max;
		}

		public static bool IsBetween(this double d, MinMax range)
		{
			return d.IsBetween(range.Min, range.Max, false);
		}

		public static bool IsBetween(this double d, MinMax range, bool exclusive)
		{
			return d.IsBetween(range.Min, range.Max, exclusive);
		}

		public static double Wrap(this double d, double min, double max)
		{
			double difference = max - min;

			while (d < min)
				d += difference;

			while (d >= max)
				d -= difference;

			return d;
		}

		public static double Wrap(this double d, MinMax range)
		{
			return d.Wrap(range.Min, range.Max);
		}

		public static double Clamp(this double d, double min, double max)
		{
			return Math.Min(Math.Max(d, min), max);
		}

		public static double Clamp(this double d, MinMax range)
		{
			return d.Clamp(range.Min, range.Max);
		}

		public static double Scale(this double d, double currentMin, double currentMax, double targetMin, double targetMax)
		{
			return (d - currentMin) / (currentMax - currentMin) * (targetMax - targetMin) + targetMin;
		}

		public static double Scale(this double d, MinMax currentRange, MinMax targetRange)
		{
			return d.Scale(currentRange.Min, currentRange.Max, targetRange.Min, targetRange.Max);
		}

		public static int Sign(this double d)
		{
			return d >= 0d ? 1 : -1;
		}

		public static double SetSign(this double d, bool sign)
		{
			return Math.Abs(d) * sign.Sign();
		}
	}
}
