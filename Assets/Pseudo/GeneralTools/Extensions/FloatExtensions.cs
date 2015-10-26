using System;
using UnityEngine;
using System.Collections;

namespace Pseudo
{
	public static class FloatExtensions
	{

		public static float Remap(this float f, float currentMin, float currentMax, float targetMin, float targetMax)
		{
			return (f - currentMin) / (currentMax - currentMin) * (targetMax - targetMin) + targetMin;
		}

		public static float PowSign(this float f, float power)
		{
			return Mathf.Abs(f).Pow(power) * f.Sign();
		}

		public static float PowSign(this float f)
		{
			return f.PowSign(2);
		}

		public static float Pow(this float f, float power)
		{
			if (float.IsNaN(f))
			{
				return 0;
			}

			if (power == 0)
			{
				return 1;
			}

			if (power == 1)
			{
				return f;
			}

			if (power == 2)
			{
				return f * f;
			}

			if (power == 3)
			{
				return f * f * f;
			}

			return Mathf.Pow(f, power);
		}

		public static float Pow(this float f)
		{
			return f.Pow(2);
		}

		public static float Round(this float f, float step)
		{
			if (float.IsNaN(f))
			{
				return 0;
			}

			if (step <= 0)
			{
				return f;
			}

			if (step == 1)
			{
				return (float)Math.Round((double)f);
			}

			return (float)Math.Round(f * (1D / step)) * step;
		}

		public static float Round(this float f)
		{
			return f.Round(1);
		}

		public static float Wrap(this float f, float wrap)
		{
			if (wrap <= 0)
			{
				return f;
			}

			while (f < 0)
			{
				f += wrap;
			}

			while (f >= wrap)
			{
				f -= wrap;
			}

			return f;
		}

		public static int Sign(this float f)
		{
			return f >= 0 ? 1 : -1;
		}

		public static float SetSign(this float f, bool sign)
		{
			return Mathf.Abs(f) * sign.Sign();
		}
	}
}
