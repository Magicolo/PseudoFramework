using UnityEngine;
using System.Collections;

namespace Pseudo
{
	public static class Vector2Extensions
	{
		const float epsilon = 0.0001F;

		public static Vector2 SetValues(this Vector2 vector, Vector2 values, Axes axes)
		{
			if ((axes & Axes.X) != 0)
				vector.x = (axes & Axes.X) != 0 ? values.x : vector.x;

			if ((axes & Axes.Y) != 0)
				vector.y = (axes & Axes.Y) != 0 ? values.y : vector.y;

			return vector;
		}

		public static Vector2 SetValues(this Vector2 vector, Vector2 values)
		{
			return vector.SetValues(values, Axes.XYZW);
		}

		public static Vector2 SetValues(this Vector2 vector, float value, Axes axes)
		{
			return vector.SetValues(new Vector2(value, value), axes);
		}

		public static Vector2 SetValues(this Vector2 vector, float value)
		{
			return vector.SetValues(new Vector2(value, value), Axes.XY);
		}

		public static Vector2 Lerp(this Vector2 vector, Vector2 target, float deltaTime, Axes axes)
		{
			if ((axes & Axes.X) != 0 && Mathf.Abs(target.x - vector.x) > epsilon)
				vector.x = Mathf.Lerp(vector.x, target.x, deltaTime);

			if ((axes & Axes.Y) != 0 && Mathf.Abs(target.y - vector.y) > epsilon)
				vector.y = Mathf.Lerp(vector.y, target.y, deltaTime);

			return vector;
		}

		public static Vector2 Lerp(this Vector2 vector, Vector2 target, float deltaTime)
		{
			return vector.Lerp(target, deltaTime, Axes.XYZW);
		}

		public static Vector2 LerpLinear(this Vector2 vector, Vector2 target, float deltaTime, Axes axes)
		{
			Vector2 difference = target - vector;
			Vector2 direction = Vector2.zero.SetValues(difference, axes);
			float distance = direction.magnitude;

			Vector2 adjustedDirection = direction.normalized * deltaTime;

			if (adjustedDirection.magnitude < distance)
				vector += Vector2.zero.SetValues(adjustedDirection, axes);
			else
				vector = vector.SetValues(target, axes);

			return vector;
		}

		public static Vector2 LerpLinear(this Vector2 vector, Vector2 target, float deltaTime)
		{
			return vector.LerpLinear(target, deltaTime, Axes.XYZW);
		}

		public static Vector2 LerpAngles(this Vector2 vector, Vector2 targetAngles, float deltaTime, Axes axes)
		{
			if ((axes & Axes.X) != 0 && Mathf.Abs(targetAngles.x - vector.x) > epsilon)
				vector.x = Mathf.LerpAngle(vector.x, targetAngles.x, deltaTime);

			if ((axes & Axes.Y) != 0 && Mathf.Abs(targetAngles.y - vector.y) > epsilon)
				vector.y = Mathf.LerpAngle(vector.y, targetAngles.y, deltaTime);

			return vector;
		}

		public static Vector2 LerpAngles(this Vector2 vector, Vector2 targetAngles, float deltaTime)
		{
			return vector.LerpAngles(targetAngles, deltaTime, Axes.XYZW);
		}

		public static Vector2 LerpAnglesLinear(this Vector2 vector, Vector2 targetAngles, float deltaTime, Axes axes)
		{
			Vector2 difference = new Vector2(Mathf.DeltaAngle(vector.x, targetAngles.x), Mathf.DeltaAngle(vector.y, targetAngles.y));
			Vector2 direction = Vector2.zero.SetValues(difference, axes);
			float distance = direction.magnitude * Mathf.Rad2Deg;

			Vector2 adjustedDirection = direction.normalized * deltaTime;

			if (adjustedDirection.magnitude < distance)
			{
				vector += Vector2.zero.SetValues(adjustedDirection, axes);
			}
			else
			{
				vector = vector.SetValues(targetAngles, axes);
			}

			return vector;
		}

		public static Vector2 LerpAnglesLinear(this Vector2 vector, Vector2 targetAngles, float deltaTime)
		{
			return vector.LerpAnglesLinear(targetAngles, deltaTime, Axes.XYZW);
		}

		public static Vector2 Oscillate(this Vector2 vector, Vector2 frequency, Vector2 amplitude, Vector2 center, float time, float offset, Axes axes)
		{
			if ((axes & Axes.X) != 0)
				vector.x = center.x + amplitude.x * Mathf.Sin(frequency.x * time + offset);

			if ((axes & Axes.Y) != 0)
				vector.y = center.y + amplitude.y * Mathf.Sin(frequency.y * time + offset);

			return vector;
		}

		public static Vector2 Oscillate(this Vector2 vector, float frequency, float amplitude, float center, float time, float offset, Axes axes)
		{
			return vector.Oscillate(new Vector2(frequency, frequency), new Vector2(amplitude, amplitude), new Vector2(center, center), time, offset, axes);
		}

		public static Vector2 Mult(this Vector2 vector, Vector2 values, Axes axes)
		{
			if ((axes & Axes.X) != 0)
				vector.x *= values.x;

			if ((axes & Axes.Y) != 0)
				vector.y *= values.y;

			return vector;
		}

		public static Vector2 Mult(this Vector2 vector, Vector2 values)
		{
			return vector.Mult(values, Axes.XYZW);
		}

		public static Vector2 Div(this Vector2 vector, Vector2 values, Axes axes)
		{
			if ((axes & Axes.X) != 0)
				vector.x /= values.x;

			if ((axes & Axes.Y) != 0)
				vector.y /= values.y;

			return vector;
		}

		public static Vector2 Div(this Vector2 vector, Vector2 values)
		{
			return vector.Div(values, Axes.XYZW);
		}

		public static Vector2 Pow(this Vector2 vector, float power, Axes axes)
		{
			if ((axes & Axes.X) != 0)
				vector.x = Mathf.Pow(vector.x, power);

			if ((axes & Axes.Y) != 0)
				vector.y = Mathf.Pow(vector.y, power);

			return vector;
		}

		public static Vector2 Pow(this Vector2 vector, float power)
		{
			return vector.Pow(power, Axes.XYZW);
		}

		public static Vector2 Round(this Vector2 vector, float step, Axes axes)
		{
			if ((axes & Axes.X) != 0)
				vector.x = vector.x.Round(step);

			if ((axes & Axes.Y) != 0)
				vector.y = vector.y.Round(step);

			return vector;
		}

		public static Vector2 Round(this Vector2 vector, float step)
		{
			return vector.Round(step, Axes.XYZW);
		}

		public static Vector2 Round(this Vector2 vector)
		{
			return vector.Round(1f, Axes.XYZW);
		}

		public static Vector2 Rotate(this Vector2 vector, float angle)
		{
			angle %= 360;

			return Quaternion.AngleAxis(angle, Vector3.forward) * vector;
		}

		public static Vector2 ClampMagnitude(this Vector2 vector, float min, float max)
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

		public static float Angle(this Vector2 vector)
		{
			if (vector.x == 0f)
				return vector.y >= 0f ? 90f : 270f;
			else if (vector.y == 0f)
				return vector.x >= 0f ? 0f : 180f;

			float angle = Mathf.Rad2Deg * Mathf.Atan(vector.y / vector.x);

			switch (vector.GetQuadrant())
			{
				case 2:
					angle += 180f;
					break;
				case 3:
					angle += 180f;
					break;
				case 4:
					angle += 360f;
					break;
			}

			return angle;
		}

		public static int GetQuadrant(this Vector2 vector)
		{
			if (vector.x >= 0f)
				return vector.y >= 0f ? 1 : 4;
			else
				return vector.y >= 0f ? 2 : 3;
		}

		public static Vector2 ToPolar(this Vector2 vector)
		{
			float sqrMagnitude = vector.sqrMagnitude;

			if (sqrMagnitude <= 0f)
				return Vector2.zero;
			else
				return new Vector2(Mathf.Sqrt(sqrMagnitude), vector.Angle());
		}

		public static Vector2 ToCartesian(this Vector2 vector)
		{
			if (vector.x <= 0f)
				return Vector2.zero;
			else if (vector.y == 0f)
				return new Vector2(vector.x, 0f);
			else if (vector.y == 90f)
				return new Vector2(0f, vector.x);
			else if (vector.y == 180f)
				return new Vector2(-vector.x, 0f);
			else if (vector.y == 270f)
				return new Vector2(0f, -vector.x);
			else
				return new Vector2(vector.x * Mathf.Cos(vector.y * Mathf.Deg2Rad), vector.x * Mathf.Sin(vector.y * Mathf.Deg2Rad));
		}

		public static Vector3 ToVector3(this Vector2 vector)
		{
			return vector;
		}

		public static Vector4 ToVector4(this Vector2 vector)
		{
			return vector;
		}
	}
}
