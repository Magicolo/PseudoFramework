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

		public static Vector4 Oscillate(this Vector4 vector, Vector4 frequency, Vector4 amplitude, Vector4 center, Vector4 offset, float time, Axes axes)
		{
			if ((axes & Axes.X) != 0)
				vector.x = center.x + amplitude.x * Mathf.Sin(frequency.x * time + offset.x);

			if ((axes & Axes.Y) != 0)
				vector.y = center.y + amplitude.y * Mathf.Sin(frequency.y * time + offset.y);

			if ((axes & Axes.Z) != 0)
				vector.z = center.z + amplitude.z * Mathf.Sin(frequency.z * time + offset.z);

			if ((axes & Axes.W) != 0)
				vector.w = center.w + amplitude.w * Mathf.Sin(frequency.w * time + offset.w);

			return vector;
		}

		public static Vector4 Oscillate(this Vector4 vector, float frequency, float amplitude, float center, float offset, float time, Axes axes)
		{
			return vector.Oscillate(
				new Vector4(frequency, frequency, frequency, frequency),
				new Vector4(amplitude, amplitude, amplitude, amplitude),
				new Vector4(center, center, center, center),
				new Vector4(offset, offset, offset, offset),
				time, axes);
		}

		public static float Distance(this Vector4 vector, Vector4 target)
		{
			return Vector4.Distance(vector, target);
		}

		public static float Distance(this Vector4 vector, Vector4 target, Axes axes)
		{
			float distance = 0f;

			switch (axes)
			{
				case Axes.X:
					distance = Mathf.Abs(vector.x - target.x);
					break;
				case Axes.Y:
					distance = Mathf.Abs(vector.y - target.y);
					break;
				case Axes.Z:
					distance = Mathf.Abs(vector.z - target.z);
					break;
				case Axes.W:
					distance = Mathf.Abs(vector.w - target.w);
					break;
				case Axes.XY:
					distance = Vector2.Distance(vector, target);
					break;
				case Axes.XZ:
					distance = Vector2.Distance(new Vector2(vector.x, vector.z), new Vector2(target.x, target.z));
					break;
				case Axes.YZ:
					distance = Vector2.Distance(new Vector2(vector.y, vector.z), new Vector2(target.y, target.z));
					break;
				case Axes.XYZ:
					distance = Vector3.Distance(vector, target);
					break;
				case Axes.XW:
					distance = Vector2.Distance(new Vector2(vector.x, vector.w), new Vector2(target.x, target.w));
					break;
				case Axes.YW:
					distance = Vector2.Distance(new Vector2(vector.y, vector.w), new Vector2(target.y, target.w));
					break;
				case Axes.XYW:
					distance = Vector3.Distance(new Vector3(vector.x, vector.y, vector.w), new Vector3(target.x, target.y, target.w));
					break;
				case Axes.ZW:
					distance = Vector2.Distance(new Vector2(vector.z, vector.w), new Vector2(target.z, target.w));
					break;
				case Axes.XZW:
					distance = Vector3.Distance(new Vector3(vector.x, vector.z, vector.w), new Vector3(target.x, target.z, target.w));
					break;
				case Axes.YZW:
					distance = Vector3.Distance(new Vector3(vector.y, vector.z, vector.w), new Vector3(target.y, target.z, target.w));
					break;
				case Axes.XYZW:
					distance = Vector4.Distance(vector, target);
					break;
			}

			return distance;
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

		public static Vector4 Floor(this Vector4 vector, Axes axes)
		{
			if ((axes & Axes.X) != 0)
				vector.x = Mathf.Floor(vector.x);

			if ((axes & Axes.Y) != 0)
				vector.y = Mathf.Floor(vector.y);

			if ((axes & Axes.Z) != 0)
				vector.z = Mathf.Floor(vector.z);

			if ((axes & Axes.W) != 0)
				vector.w = Mathf.Floor(vector.w);

			return vector;
		}

		public static Vector4 Floor(this Vector4 vector)
		{
			return vector.Floor(Axes.XYZW);
		}

		public static Vector4 Ceil(this Vector4 vector, Axes axes)
		{
			if ((axes & Axes.X) != 0)
				vector.x = Mathf.Ceil(vector.x);

			if ((axes & Axes.Y) != 0)
				vector.y = Mathf.Ceil(vector.y);

			if ((axes & Axes.Z) != 0)
				vector.z = Mathf.Ceil(vector.z);

			if ((axes & Axes.W) != 0)
				vector.w = Mathf.Ceil(vector.w);

			return vector;
		}

		public static Vector4 Ceil(this Vector4 vector)
		{
			return vector.Ceil(Axes.XYZW);
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
