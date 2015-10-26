using UnityEngine;
using System.Collections;

namespace Pseudo
{
	public static class Vector4Extensions
	{

		const float epsilon = 0.001F;

		public static Vector4 SetValues(this Vector4 vector, Vector4 values, Axes axes)
		{
			vector.x = axes.Contains(Axes.X) ? values.x : vector.x;
			vector.y = axes.Contains(Axes.Y) ? values.y : vector.y;
			vector.z = axes.Contains(Axes.Z) ? values.z : vector.z;
			vector.w = axes.Contains(Axes.W) ? values.w : vector.w;

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

		public static Vector4 Lerp(this Vector4 vector, Vector4 target, float time, Axes axes)
		{
			vector.x = axes.Contains(Axes.X) && Mathf.Abs(target.x - vector.x) > epsilon ? Mathf.Lerp(vector.x, target.x, time) : vector.x;
			vector.y = axes.Contains(Axes.Y) && Mathf.Abs(target.y - vector.y) > epsilon ? Mathf.Lerp(vector.y, target.y, time) : vector.y;
			vector.z = axes.Contains(Axes.Z) && Mathf.Abs(target.z - vector.z) > epsilon ? Mathf.Lerp(vector.z, target.z, time) : vector.z;
			vector.w = axes.Contains(Axes.W) && Mathf.Abs(target.w - vector.w) > epsilon ? Mathf.Lerp(vector.w, target.w, time) : vector.w;

			return vector;
		}

		public static Vector4 Lerp(this Vector4 vector, Vector4 target, float time)
		{
			return vector.Lerp(target, time, Axes.XYZW);
		}

		public static Vector4 LerpLinear(this Vector4 vector, Vector4 target, float time, Axes axes)
		{
			Vector4 difference = target - vector;
			Vector4 direction = Vector4.zero.SetValues(difference, axes);
			float distance = direction.magnitude;

			Vector4 adjustedDirection = direction.normalized * time;

			if (adjustedDirection.magnitude < distance)
			{
				vector += Vector4.zero.SetValues(adjustedDirection, axes);
			}
			else
			{
				vector = vector.SetValues(target, axes);
			}

			return vector;
		}

		public static Vector4 LerpLinear(this Vector4 vector, Vector4 target, float time)
		{
			return vector.LerpLinear(target, time, Axes.XYZW);
		}

		public static Vector4 LerpAngles(this Vector4 vector, Vector4 targetAngles, float time, Axes axes)
		{
			vector.x = axes.Contains(Axes.X) && Mathf.Abs(targetAngles.x - vector.x) > epsilon ? Mathf.LerpAngle(vector.x, targetAngles.x, time) : vector.x;
			vector.y = axes.Contains(Axes.Y) && Mathf.Abs(targetAngles.y - vector.y) > epsilon ? Mathf.LerpAngle(vector.y, targetAngles.y, time) : vector.y;
			vector.z = axes.Contains(Axes.Z) && Mathf.Abs(targetAngles.z - vector.z) > epsilon ? Mathf.LerpAngle(vector.z, targetAngles.z, time) : vector.z;
			vector.w = axes.Contains(Axes.W) && Mathf.Abs(targetAngles.w - vector.w) > epsilon ? Mathf.LerpAngle(vector.w, targetAngles.w, time) : vector.w;

			return vector;
		}

		public static Vector4 LerpAngles(this Vector4 vector, Vector4 targetAngles, float time)
		{
			return vector.LerpAngles(targetAngles, time, Axes.XYZW);
		}

		public static Vector4 LerpAnglesLinear(this Vector4 vector, Vector4 targetAngles, float time, Axes axes)
		{
			Vector4 difference = new Vector4(Mathf.DeltaAngle(vector.x, targetAngles.x), Mathf.DeltaAngle(vector.y, targetAngles.y), Mathf.DeltaAngle(vector.z, targetAngles.z), Mathf.DeltaAngle(vector.w, targetAngles.w));
			Vector4 direction = Vector4.zero.SetValues(difference, axes);
			float distance = direction.magnitude * Mathf.Rad2Deg;

			Vector4 adjustedDirection = direction.normalized * time;

			if (adjustedDirection.magnitude < distance)
			{
				vector += Vector4.zero.SetValues(adjustedDirection, axes);
			}
			else
			{
				vector = vector.SetValues(targetAngles, axes);
			}

			return vector;
		}

		public static Vector4 LerpAnglesLinear(this Vector4 vector, Vector4 targetAngles, float time)
		{
			return vector.LerpAnglesLinear(targetAngles, time, Axes.XYZW);
		}

		public static Vector4 Oscillate(this Vector4 vector, Vector4 frequency, Vector4 amplitude, Vector4 center, float offset, Axes axes)
		{
			vector.x = axes.Contains(Axes.X) ? center.x + amplitude.x * Mathf.Sin(frequency.x * Time.time + offset) : vector.x;
			vector.y = axes.Contains(Axes.Y) ? center.y + amplitude.y * Mathf.Sin(frequency.y * Time.time + offset) : vector.y;
			vector.z = axes.Contains(Axes.Z) ? center.z + amplitude.z * Mathf.Sin(frequency.z * Time.time + offset) : vector.z;
			vector.w = axes.Contains(Axes.W) ? center.w + amplitude.w * Mathf.Sin(frequency.w * Time.time + offset) : vector.w;

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

		public static Vector4 Mult(this Vector4 vector, Vector4 otherVector, Axes axes)
		{
			vector.x = axes.Contains(Axes.X) ? vector.x * otherVector.x : vector.x;
			vector.y = axes.Contains(Axes.Y) ? vector.y * otherVector.y : vector.y;
			vector.z = axes.Contains(Axes.Z) ? vector.z * otherVector.z : vector.z;
			vector.w = axes.Contains(Axes.W) ? vector.w * otherVector.w : vector.w;

			return vector;
		}

		public static Vector4 Mult(this Vector4 vector, Vector4 otherVector)
		{
			return vector.Mult(otherVector, Axes.XYZW);
		}

		public static Vector4 Mult(this Vector4 vector, Vector2 otherVector, Axes axes)
		{
			return vector.Mult((Vector4)otherVector, axes);
		}

		public static Vector4 Mult(this Vector4 vector, Vector2 otherVector)
		{
			return vector.Mult((Vector4)otherVector, Axes.XYZW);
		}

		public static Vector4 Mult(this Vector4 vector, Vector3 otherVector, Axes axes)
		{
			return vector.Mult((Vector4)otherVector, axes);
		}

		public static Vector4 Mult(this Vector4 vector, Vector3 otherVector)
		{
			return vector.Mult((Vector4)otherVector, Axes.XYZW);
		}

		public static Vector4 Div(this Vector4 vector, Vector4 otherVector, Axes axes)
		{
			vector.x = axes.Contains(Axes.X) ? vector.x / otherVector.x : vector.x;
			vector.y = axes.Contains(Axes.Y) ? vector.y / otherVector.y : vector.y;
			vector.z = axes.Contains(Axes.Z) ? vector.z / otherVector.z : vector.z;
			vector.w = axes.Contains(Axes.W) ? vector.w / otherVector.w : vector.w;

			return vector;
		}

		public static Vector4 Div(this Vector4 vector, Vector4 otherVector)
		{
			return vector.Div(otherVector, Axes.XYZW);
		}

		public static Vector4 Div(this Vector4 vector, Vector2 otherVector, Axes axes)
		{
			return vector.Div((Vector4)otherVector, axes);
		}

		public static Vector4 Div(this Vector4 vector, Vector2 otherVector)
		{
			return vector.Div((Vector4)otherVector, Axes.XYZW);
		}

		public static Vector4 Div(this Vector4 vector, Vector3 otherVector, Axes axes)
		{
			return vector.Div((Vector4)otherVector, axes);
		}

		public static Vector4 Div(this Vector4 vector, Vector3 otherVector)
		{
			return vector.Div((Vector4)otherVector, Axes.XYZW);
		}

		public static Vector4 Pow(this Vector4 vector, float power, Axes axes)
		{
			vector.x = axes.Contains(Axes.X) ? vector.x.Pow(power) : vector.x;
			vector.y = axes.Contains(Axes.Y) ? vector.y.Pow(power) : vector.y;
			vector.z = axes.Contains(Axes.Z) ? vector.z.Pow(power) : vector.z;
			vector.w = axes.Contains(Axes.W) ? vector.w.Pow(power) : vector.w;

			return vector;
		}

		public static Vector4 Pow(this Vector4 vector, float power)
		{
			return vector.Pow(power, Axes.XYZW);
		}

		public static Vector4 Round(this Vector4 vector, float step, Axes axes)
		{
			vector.x = axes.Contains(Axes.X) ? vector.x.Round(step) : vector.x;
			vector.y = axes.Contains(Axes.Y) ? vector.y.Round(step) : vector.y;
			vector.z = axes.Contains(Axes.Z) ? vector.z.Round(step) : vector.z;
			vector.w = axes.Contains(Axes.W) ? vector.w.Round(step) : vector.w;

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

			if (axes.Contains(Axes.X))
			{
				average += vector.x;
				axisCount += 1;
			}

			if (axes.Contains(Axes.Y))
			{
				average += vector.y;
				axisCount += 1;
			}

			if (axes.Contains(Axes.Z))
			{
				average += vector.z;
				axisCount += 1;
			}

			if (axes.Contains(Axes.W))
			{
				average += vector.w;
				axisCount += 1;
			}

			return average / axisCount;
		}

		public static float Average(this Vector4 vector)
		{
			return ((Vector4)vector).Average(Axes.XYZW);
		}

		public static Vector4 ClampMagnitude(this Vector4 vector, float min, float max)
		{
			Vector4 clamped = vector;
			float sqrMagniture = vector.sqrMagnitude;
			float sqrMin = min * min;
			float sqrMax = max * max;

			if (sqrMagniture < sqrMin)
			{
				clamped = vector.normalized * min;
			}
			else if (sqrMagniture > sqrMax)
			{
				clamped = vector.normalized * max;
			}

			return clamped;
		}
	}
}
