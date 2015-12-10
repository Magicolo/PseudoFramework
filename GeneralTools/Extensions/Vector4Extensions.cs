using UnityEngine;
using System.Collections;

namespace Pseudo
{
	public static class Vector4Extensions
	{
		const float epsilon = 0.0001F;

		public static Vector4 SetValues(this Vector4 vector, Vector4 values, Axes axes)
		{
			if ((axes & Axes.X) != 0)
				vector.x = values.x;

			if ((axes & Axes.Y) != 0)
				vector.y = values.y;

			if ((axes & Axes.Z) != 0)
				vector.z = values.z;

			if ((axes & Axes.W) != 0)
				vector.w = values.w;

			return vector;
		}

		public static Vector4 SetValues(this Vector4 vector, Vector4 values)
		{
			return vector.SetValues(values, Axes.XYZW);
		}

		public static Vector4 SetValues(this Vector4 vector, float value, Axes axes)
		{
			return vector.SetValues(new Vector4(value, value, value, value), axes);
		}

		public static Vector4 SetValues(this Vector4 vector, float value)
		{
			return vector.SetValues(new Vector4(value, value, value, value), Axes.XYZW);
		}

		public static Vector4 Lerp(this Vector4 vector, Vector4 target, float deltaTime, Axes axes)
		{
			if ((axes & Axes.X) != 0 && Mathf.Abs(target.x - vector.x) > epsilon)
				vector.x = Mathf.Lerp(vector.x, target.x, deltaTime);

			if ((axes & Axes.Y) != 0 && Mathf.Abs(target.y - vector.y) > epsilon)
				vector.y = Mathf.Lerp(vector.y, target.y, deltaTime);

			if ((axes & Axes.Z) != 0 && Mathf.Abs(target.z - vector.z) > epsilon)
				vector.z = Mathf.Lerp(vector.z, target.z, deltaTime);

			if ((axes & Axes.W) != 0 && Mathf.Abs(target.w - vector.w) > epsilon)
				vector.w = Mathf.Lerp(vector.w, target.w, deltaTime);

			return vector;
		}

		public static Vector4 Lerp(this Vector4 vector, Vector4 target, float deltaTime)
		{
			return vector.Lerp(target, deltaTime, Axes.XYZW);
		}

		public static Vector4 LerpLinear(this Vector4 vector, Vector4 target, float deltaTime, Axes axes)
		{
			Vector4 difference = target - vector;
			Vector4 direction = Vector4.zero.SetValues(difference, axes);
			float distance = direction.magnitude;

			Vector4 adjustedDirection = direction.normalized * deltaTime;

			if (adjustedDirection.magnitude < distance)
				vector += Vector4.zero.SetValues(adjustedDirection, axes);
			else
				vector = vector.SetValues(target, axes);

			return vector;
		}

		public static Vector4 LerpLinear(this Vector4 vector, Vector4 target, float deltaTime)
		{
			return vector.LerpLinear(target, deltaTime, Axes.XYZW);
		}

		public static Vector4 Oscillate(this Vector4 vector, Vector4 frequency, Vector4 amplitude, Vector4 center, float offset, Axes axes)
		{
			if ((axes & Axes.X) != 0)
				vector.x = center.x + amplitude.x * Mathf.Sin(frequency.x * Time.time + offset);

			if ((axes & Axes.Y) != 0)
				vector.y = center.y + amplitude.y * Mathf.Sin(frequency.y * Time.time + offset);

			if ((axes & Axes.Z) != 0)
				vector.z = center.z + amplitude.z * Mathf.Sin(frequency.z * Time.time + offset);

			if ((axes & Axes.W) != 0)
				vector.w = center.w + amplitude.w * Mathf.Sin(frequency.w * Time.time + offset);

			return vector;
		}

		public static Vector4 Oscillate(this Vector4 vector, Vector4 frequency, Vector4 amplitude, Vector4 center, float offset)
		{
			return vector.Oscillate(frequency, amplitude, center, offset, Axes.XYZW);
		}

		public static Vector4 Oscillate(this Vector4 vector, Vector4 frequency, Vector4 amplitude, Vector4 center, Axes axes)
		{
			return vector.Oscillate(frequency, amplitude, center, 0, axes);
		}

		public static Vector4 Oscillate(this Vector4 vector, Vector4 frequency, Vector4 amplitude, Vector4 center)
		{
			return vector.Oscillate(frequency, amplitude, center, 0, Axes.XYZW);
		}

		public static Vector4 Mult(this Vector4 vector, Vector4 values, Axes axes)
		{
			if ((axes & Axes.X) != 0)
				vector.x *= values.x;

			if ((axes & Axes.Y) != 0)
				vector.y *= values.y;

			if ((axes & Axes.Z) != 0)
				vector.z *= values.z;

			if ((axes & Axes.W) != 0)
				vector.w *= values.w;

			return vector;
		}

		public static Vector4 Mult(this Vector4 vector, Vector4 values)
		{
			return vector.Mult(values, Axes.XYZW);
		}

		public static Vector4 Div(this Vector4 vector, Vector4 values, Axes axes)
		{
			if ((axes & Axes.X) != 0)
				vector.x /= values.x;

			if ((axes & Axes.Y) != 0)
				vector.y /= values.y;

			if ((axes & Axes.Z) != 0)
				vector.z /= values.z;

			if ((axes & Axes.W) != 0)
				vector.w /= values.w;

			return vector;
		}

		public static Vector4 Div(this Vector4 vector, Vector4 values)
		{
			return vector.Div(values, Axes.XYZW);
		}

		public static Vector4 Pow(this Vector4 vector, float power, Axes axes)
		{
			if ((axes & Axes.X) != 0)
				vector.x = Mathf.Pow(vector.x, power);

			if ((axes & Axes.Y) != 0)
				vector.y = Mathf.Pow(vector.y, power);

			if ((axes & Axes.Z) != 0)
				vector.z = Mathf.Pow(vector.z, power);

			if ((axes & Axes.W) != 0)
				vector.w = Mathf.Pow(vector.w, power);

			return vector;
		}

		public static Vector4 Pow(this Vector4 vector, float power)
		{
			return vector.Pow(power, Axes.XYZW);
		}

		public static Vector4 Round(this Vector4 vector, float step, Axes axes)
		{
			if ((axes & Axes.X) != 0)
				vector.x = vector.x.Round(step);

			if ((axes & Axes.Y) != 0)
				vector.y = vector.y.Round(step);

			if ((axes & Axes.Z) != 0)
				vector.z = vector.z.Round(step);

			if ((axes & Axes.W) != 0)
				vector.w = vector.w.Round(step);

			return vector;
		}

		public static Vector4 Round(this Vector4 vector, float step)
		{
			return vector.Round(step, Axes.XYZW);
		}

		public static Vector4 Round(this Vector4 vector)
		{
			return vector.Round(1, Axes.XYZW);
		}

		public static float Average(this Vector4 vector, Axes axes)
		{
			float average = 0;
			int axisCount = 0;

			if ((axes & Axes.X) != 0)
			{
				average += vector.x;
				axisCount += 1;
			}

			if ((axes & Axes.Y) != 0)
			{
				average += vector.y;
				axisCount += 1;
			}

			if ((axes & Axes.Z) != 0)
			{
				average += vector.z;
				axisCount += 1;
			}

			if ((axes & Axes.W) != 0)
			{
				average += vector.w;
				axisCount += 1;
			}

			return average / axisCount;
		}

		public static float Average(this Vector4 vector)
		{
			return vector.Average(Axes.XYZW);
		}

		public static Vector4 ClampMagnitude(this Vector4 vector, float min, float max)
		{
			float sqrMagniture = vector.sqrMagnitude;
			float sqrMin = min * min;
			float sqrMax = max * max;

			if (sqrMagniture < sqrMin)
				vector = vector.normalized * min;
			else if (sqrMagniture > sqrMax)
				vector = vector.normalized * max;

			return vector;
		}

		public static Quaternion ToQuaternion(this Vector4 vector)
		{
			return new Quaternion(vector.x, vector.y, vector.z, vector.w);
		}
	}
}
