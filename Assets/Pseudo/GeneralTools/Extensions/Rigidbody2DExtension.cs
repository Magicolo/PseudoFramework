using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo
{
	public static class Rigidbody2DExtension
	{
		#region Velocity
		public static void SetVelocity(this Rigidbody2D rigidbody, Vector2 velocity, Axes axes = Axes.XY)
		{
			rigidbody.velocity = rigidbody.velocity.SetValues(velocity, axes);
		}

		public static void SetVelocity(this Rigidbody2D rigidbody, float velocity, Axes axes = Axes.XY)
		{
			rigidbody.SetVelocity(new Vector2(velocity, velocity), axes);
		}

		public static void Accelerate(this Rigidbody2D rigidbody, Vector2 speed, float deltaTime, Axes axes = Axes.XY)
		{
			rigidbody.SetVelocity((rigidbody.velocity + speed * deltaTime), axes);
		}

		public static void Accelerate(this Rigidbody2D rigidbody, float speed, float deltaTime, Axes axes = Axes.XY)
		{
			rigidbody.Accelerate(new Vector2(speed, speed), deltaTime, axes);
		}

		public static void AccelerateTowards(this Rigidbody2D rigidbody, Vector2 targetSpeed, float deltaTime, InterpolationModes interpolation = InterpolationModes.Quadratic, Axes axes = Axes.XY)
		{
			switch (interpolation)
			{
				case InterpolationModes.Quadratic:
					rigidbody.SetVelocity(rigidbody.velocity.Lerp(targetSpeed, deltaTime, axes), axes);
					break;
				case InterpolationModes.Linear:
					rigidbody.SetVelocity(rigidbody.velocity.LerpLinear(targetSpeed, deltaTime, axes), axes);
					break;
			}
		}

		public static void AccelerateTowards(this Rigidbody2D rigidbody, float targetSpeed, float deltaTime, InterpolationModes interpolation = InterpolationModes.Quadratic, Axes axes = Axes.XY)
		{
			rigidbody.AccelerateTowards(new Vector2(targetSpeed, targetSpeed), deltaTime, interpolation, axes);
		}

		public static void OscillateVelocity(this Rigidbody2D rigidbody, Vector2 frequency, Vector2 amplitude, Vector2 center, float time, Axes axes = Axes.XY)
		{
			rigidbody.SetVelocity(rigidbody.velocity.Oscillate(frequency, amplitude, center, time, rigidbody.GetInstanceID() / 1000, axes), axes);
		}

		public static void OscillateVelocity(this Rigidbody2D rigidbody, float frequency, float amplitude, float center, float time, Axes axes = Axes.XY)
		{
			OscillateVelocity(rigidbody, new Vector2(frequency, frequency), new Vector2(amplitude, amplitude), new Vector2(center, center), time, axes);
		}
		#endregion

		#region Position
		public static void SetPosition(this Rigidbody2D rigidbody, Vector2 position, Axes axes = Axes.XY)
		{
			rigidbody.MovePosition(rigidbody.transform.position.ToVector2().SetValues(position, axes));
		}

		public static void SetPosition(this Rigidbody2D rigidbody, float position, Axes axes = Axes.XY)
		{
			rigidbody.SetPosition(new Vector2(position, position), axes);
		}

		public static void Translate(this Rigidbody2D rigidbody, Vector2 translation, Axes axes = Axes.XY)
		{
			rigidbody.SetPosition(rigidbody.transform.position + translation.ToVector3(), axes);
		}

		public static void Translate(this Rigidbody2D rigidbody, float translation, Axes axes = Axes.XY)
		{
			rigidbody.Translate(new Vector2(translation, translation), axes);
		}

		public static void TranslateTowards(this Rigidbody2D rigidbody, Vector2 targetPosition, float deltaTime, InterpolationModes interpolation = InterpolationModes.Quadratic, Axes axes = Axes.XY)
		{
			switch (interpolation)
			{
				case InterpolationModes.Quadratic:
					rigidbody.SetPosition(rigidbody.transform.position.ToVector2().Lerp(targetPosition, deltaTime, axes), axes);
					break;
				case InterpolationModes.Linear:
					rigidbody.SetPosition(rigidbody.transform.position.ToVector2().LerpLinear(targetPosition, deltaTime, axes), axes);
					break;
			}
		}

		public static void TranslateTowards(this Rigidbody2D rigidbody, float targetPosition, float deltaTime, InterpolationModes interpolation = InterpolationModes.Quadratic, Axes axes = Axes.XY)
		{
			rigidbody.TranslateTowards(new Vector2(targetPosition, targetPosition), deltaTime, interpolation, axes);
		}

		public static void OscillatePosition(this Rigidbody2D rigidbody, Vector2 frequency, Vector2 amplitude, Vector2 center, float time, Axes axes = Axes.XY)
		{
			rigidbody.SetPosition(rigidbody.transform.position.ToVector2().Oscillate(frequency, amplitude, center, time, rigidbody.transform.GetInstanceID() / 1000, axes), axes);
		}

		public static void OscillatePosition(this Rigidbody2D rigidbody, float frequency, float amplitude, float center, float time, Axes axes = Axes.XY)
		{
			rigidbody.OscillatePosition(new Vector2(frequency, frequency), new Vector2(amplitude, amplitude), new Vector2(center, center), time, axes);
		}
		#endregion

		#region Rotation
		public static void SetEulerAngle(this Rigidbody2D rigidbody, float angle)
		{
			rigidbody.MoveRotation(angle);
		}

		public static void Rotate(this Rigidbody2D rigidbody, float rotation)
		{
			rigidbody.SetEulerAngle(rigidbody.transform.eulerAngles.z + rotation);
		}

		public static void RotateTowards(this Rigidbody2D rigidbody, float targetAngle, float deltaTime, InterpolationModes interpolation = InterpolationModes.Quadratic)
		{
			switch (interpolation)
			{
				case InterpolationModes.Quadratic:
					rigidbody.SetEulerAngle(rigidbody.transform.eulerAngles.LerpAngles(new Vector3(targetAngle, targetAngle, targetAngle), deltaTime, Axes.Z).z);
					break;
				case InterpolationModes.Linear:
					rigidbody.SetEulerAngle(rigidbody.transform.eulerAngles.LerpAnglesLinear(new Vector3(targetAngle, targetAngle, targetAngle), deltaTime, Axes.Z).z);
					break;
			}
		}

		public static void OscillateEulerAngles(this Rigidbody2D rigidbody, float frequency, float amplitude, float center, float time)
		{
			rigidbody.SetEulerAngle(rigidbody.transform.eulerAngles.Oscillate(new Vector3(frequency, frequency, frequency), new Vector3(amplitude, amplitude, amplitude), new Vector3(center, center, center), time, rigidbody.GetInstanceID() / 1000, Axes.Z).z);
		}
		#endregion
	}
}