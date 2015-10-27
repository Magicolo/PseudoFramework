using UnityEngine;
using System;
using System.Collections;

namespace Pseudo
{
	public static class DoubleExtensions
	{
		public static double Remap(this double d, double sourceMin, double sourceMax, double targetMin, double targetMax)
		{
			return (d - sourceMin) / (sourceMax - sourceMin) * (targetMax - targetMin) + targetMin;
		}

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

		public static double Wrap(this double d, double min, double max)
		{
			double difference = max - min;

			while (d < min)
				d += difference;

			while (d >= max)
				d -= difference;

			return d;
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
