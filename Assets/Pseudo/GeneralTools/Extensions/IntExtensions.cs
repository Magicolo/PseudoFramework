using System;
using UnityEngine;

namespace Pseudo
{
	public static class IntExtensions
	{

		public static int Remap(this int i, int currentMin, int currentMax, int targetMin, int targetMax)
		{
			return (i - currentMin) / (currentMax - currentMin) * (targetMax - targetMin) + targetMin;
		}

		public static int PowSign(this int i, int power)
		{
			return Mathf.Abs(i).Pow(power) * i.Sign();
		}

		public static int PowSign(this int i)
		{
			return i.PowSign(2);
		}

		public static int Pow(this int i, int power)
		{
			if (power == 0)
			{
				return 1;
			}

			if (power == 1)
			{
				return i;
			}

			if (power == 2)
			{
				return i * i;
			}

			if (power == 3)
			{
				return i * i * i;
			}

			return (int)Mathf.Pow(i, power);
		}

		public static int Pow(this int i)
		{
			return i.Pow(2);
		}

		public static int Round(this int i, int step)
		{
			if (step <= 0)
			{
				return i;
			}

			if (step == 1)
			{
				return (int)Math.Round((double)i);
			}

			return (int)(Math.Round(i * (1D / step)) * step);
		}

		public static int Round(this int i)
		{
			return i.Round(1);
		}

		public static int Wrap(this int i, int wrap)
		{
			if (wrap <= 0)
			{
				return i;
			}

			while (i < 0)
			{
				i += wrap;
			}

			while (i >= wrap)
			{
				i -= wrap;
			}

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
