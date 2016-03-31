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

		public static void AccelerateTowards(this Rigidbody2D rigidbody, Vector2 targetSpeed, float deltaTime, Axes axes = Axes.XY)
		{
			rigidbody.SetVelocity(rigidbody.velocity.Lerp(targetSpeed, deltaTime, axes), axes);
		}

		public static void AccelerateTowards(this Rigidbody2D rigidbody, float targetSpeed, float deltaTime, Axes axes = Axes.XY)
		{
			rigidbody.AccelerateTowards(new Vector2(targetSpeed, targetSpeed), deltaTime, axes);
		}
		#endregion

		#region Position
		public static void SetPosition(this Rigidbody2D rigidbody, Vector2 position, Axes axes = Axes.XY)
		{
			rigidbody.MovePosition(rigidbody.position.SetValues(position, axes));
		}

		public static void SetPosition(this Rigidbody2D rigidbody, float position, Axes axes = Axes.XY)
		{
			rigidbody.SetPosition(new Vector2(position, position), axes);
		}

		public static void Translate(this Rigidbody2D rigidbody, Vector2 translation, Axes axes = Axes.XY)
		{
			rigidbody.SetPosition(rigidbody.position + translation, axes);
		}

		public static void Translate(this Rigidbody2D rigidbody, float translation, Axes axes = Axes.XY)
		{
			rigidbody.Translate(new Vector2(translation, translation), axes);
		}

		public static void TranslateTowards(this Rigidbody2D rigidbody, Vector2 targetPosition, float deltaTime, Axes axes = Axes.XY)
		{
			rigidbody.SetPosition(rigidbody.position.Lerp(targetPosition, deltaTime, axes), axes);
		}

		public static void TranslateTowards(this Rigidbody2D rigidbody, float targetPosition, float deltaTime, Axes axes = Axes.XY)
		{
			rigidbody.TranslateTowards(new Vector2(targetPosition, targetPosition), deltaTime, axes);
		}
		#endregion

		#region Rotation
		public static void SetEulerAngle(this Rigidbody2D rigidbody, float angle)
		{
			rigidbody.MoveRotation(angle);
		}

		public static void Rotate(this Rigidbody2D rigidbody, float rotation)
		{
			rigidbody.SetEulerAngle(rigidbody.rotation + rotation);
		}

		public static void RotateTowards(this Rigidbody2D rigidbody, float targetAngle, float deltaTime)
		{
			rigidbody.SetEulerAngle(Mathf.LerpAngle(rigidbody.rotation, targetAngle, deltaTime));
		}
		#endregion
	}
}