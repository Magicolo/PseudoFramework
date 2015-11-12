using UnityEngine;
using System.Collections;

namespace Pseudo
{
	public static class QuaternionExtensions
	{
		public static Quaternion Round(this Quaternion quaternion, float step, Axes axes)
		{
			if (step <= 0)
				return quaternion;

			if ((axes & Axes.X) != 0)
				quaternion.x = quaternion.x.Round(step);

			if ((axes & Axes.Y) != 0)
				quaternion.y = quaternion.y.Round(step);

			if ((axes & Axes.Z) != 0)
				quaternion.z = quaternion.z.Round(step);

			if ((axes & Axes.W) != 0)
				quaternion.w = quaternion.w.Round(step);

			return quaternion;
		}

		public static Quaternion Round(this Quaternion quaternion, float step)
		{
			return quaternion.Round(step, Axes.XYZW);
		}

		public static Quaternion Round(this Quaternion quaternion)
		{
			return quaternion.Round(1f, Axes.XYZW);
		}
	}
}
