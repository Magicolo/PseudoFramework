using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal.PhysicsTools
{
	public class GravityBase : PMonoBehaviour
	{
		[SerializeField, PropertyField(typeof(RangeAttribute), 0, 360)]
		float angle = 90;
		public float Angle
		{
			get { return angle; }
			set
			{
				angle = value % 360;
				force = (Vector2.right.Rotate(angle) * strength).Round(0.0001F);
				direction = force.normalized;
				hasChanged = true;
			}
		}

		[SerializeField, PropertyField(typeof(MinAttribute))]
		float strength = 20;
		public float Strength
		{
			get { return strength; }
			set
			{
				strength = value;
				force = (Vector2.right.Rotate(angle) * strength).Round(0.0001F);
				direction = force.normalized;
				hasChanged = true;
			}
		}

		[SerializeField, PropertyField]
		Vector2 direction = new Vector2(0, -1);
		public Vector2 Direction
		{
			get { return direction; }
			set
			{
				direction = value.normalized.Round(0.0001F);
				force = direction * strength;
				angle = direction.Angle();
				hasChanged = true;
			}
		}

		[SerializeField, PropertyField]
		Vector2 force = new Vector2(0, -20);
		public Vector2 Force
		{
			get { return force; }
			set
			{
				force = value;
				strength = force.magnitude;
				direction = force.normalized;
				angle = direction.Angle();
				hasChanged = true;
			}
		}

		Vector2 left;
		public Vector2 Left
		{
			get
			{
				if (hasChanged)
					UpdateForces();

				return right;
			}
		}

		Vector2 right;
		public Vector2 Right
		{
			get
			{
				if (hasChanged)
					UpdateForces();

				return right;
			}
		}

		public Vector2 Up { get { return -force; } }

		bool hasChanged = true;

		void UpdateForces()
		{
			left = force.Rotate(90).normalized;
			right = -left;

			hasChanged = false;
		}

		public Vector2 WorldToRelative(Vector2 vector)
		{
			return vector.Rotate(-Angle + 90);
		}

		public Vector2 RelativeToWorld(Vector2 vector)
		{
			return vector.Rotate(Angle - 90);
		}

		public static implicit operator Vector2(GravityBase gravity)
		{
			return gravity.Force;
		}

		public static implicit operator Vector3(GravityBase gravity)
		{
			return gravity.Force;
		}
	}
}