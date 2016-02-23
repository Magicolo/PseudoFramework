using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace Pseudo
{
	public static class RigidbodyExtensions
	{
		#region Velocity
		public static void SetVelocity(this Rigidbody rigidbody, Vector3 velocity, Axes axes = Axes.XYZ)
		{
			rigidbody.velocity = rigidbody.velocity.SetValues(velocity, axes);
		}

		public static void SetVelocity(this Rigidbody rigidbody, float velocity, Axes axes = Axes.XYZ)
		{
			rigidbody.SetVelocity(new Vector3(velocity, velocity, velocity), axes);
		}

		public static void Accelerate(this Rigidbody rigidbody, Vector3 speed, Axes axes = Axes.XYZ)
		{
			rigidbody.SetVelocity(rigidbody.velocity + speed, axes);
		}

		public static void Accelerate(this Rigidbody rigidbody, float speed, Axes axes = Axes.XYZ)
		{
			rigidbody.Accelerate(new Vector3(speed, speed, speed), axes);
		}

		public static void AccelerateTowards(this Rigidbody rigidbody, Vector3 targetSpeed, float deltaTime, Axes axes = Axes.XYZ)
		{
			rigidbody.SetVelocity(rigidbody.velocity.Lerp(targetSpeed, deltaTime, axes), axes);
		}

		public static void AccelerateTowards(this Rigidbody rigidbody, float targetSpeed, float deltaTime, Axes axes = Axes.XYZ)
		{
			rigidbody.AccelerateTowards(new Vector3(targetSpeed, targetSpeed, targetSpeed), deltaTime, axes);
		}
		#endregion

		#region Position
		public static void SetPosition(this Rigidbody rigidbody, Vector3 position, Axes axes = Axes.XYZ)
		{
			rigidbody.MovePosition(rigidbody.position.SetValues(position, axes));
		}

		public static void SetPosition(this Rigidbody rigidbody, float position, Axes axes = Axes.XYZ)
		{
			rigidbody.SetPosition(new Vector3(position, position, position), axes);
		}

		public static void Translate(this Rigidbody rigidbody, Vector3 translation, Axes axes = Axes.XYZ)
		{
			rigidbody.SetPosition(rigidbody.position + translation, axes);
		}

		public static void Translate(this Rigidbody rigidbody, float translation, Axes axes = Axes.XYZ)
		{
			rigidbody.Translate(new Vector3(translation, translation, translation), axes);
		}

		public static void TranslateTowards(this Rigidbody rigidbody, Vector3 targetPosition, float deltaTime, Axes axes = Axes.XYZ)
		{
			rigidbody.SetPosition(rigidbody.position.Lerp(targetPosition, deltaTime, axes), axes);
		}

		public static void TranslateTowards(this Rigidbody rigidbody, float targetPosition, float deltaTime, Axes axes = Axes.XYZ)
		{
			rigidbody.TranslateTowards(new Vector3(targetPosition, targetPosition, targetPosition), deltaTime, axes);
		}
		#endregion

		#region Rotation
		public static void SetEulerAngles(this Rigidbody rigidbody, Vector3 angles, Axes axes = Axes.XYZ)
		{
			rigidbody.MoveRotation(Quaternion.Euler(rigidbody.rotation.eulerAngles.SetValues(angles, axes)));
		}

		public static void SetEulerAngles(this Rigidbody rigidbody, float angle, Axes axes = Axes.XYZ)
		{
			rigidbody.SetEulerAngles(new Vector3(angle, angle, angle), axes);
		}

		public static void Rotate(this Rigidbody rigidbody, Vector3 rotation, Axes axes = Axes.XYZ)
		{
			rigidbody.SetEulerAngles(rigidbody.rotation.eulerAngles + rotation, axes);
		}

		public static void Rotate(this Rigidbody rigidbody, float rotation, Axes axes = Axes.XYZ)
		{
			rigidbody.Rotate(new Vector3(rotation, rotation, rotation), axes);
		}

		public static void RotateTowards(this Rigidbody rigidbody, Vector3 targetAngles, float deltaTime, Axes axes = Axes.XYZ)
		{
			rigidbody.SetEulerAngles(rigidbody.rotation.eulerAngles.LerpAngles(targetAngles, deltaTime, axes), axes);
		}

		public static void RotateTowards(this Rigidbody rigidbody, float targetAngle, float deltaTime, Axes axes = Axes.XYZ)
		{
			rigidbody.RotateTowards(new Vector3(targetAngle, targetAngle, targetAngle), deltaTime, axes);
		}
		#endregion
	}
}

