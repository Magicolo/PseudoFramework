using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal.PhysicsTools
{
	public class GravityBase : PMonoBehaviour
	{
		[SerializeField, PropertyField(typeof(RangeAttribute), 0, 360)]
		float _angle = 90;
		public float Angle
		{
			get { return _angle; }
			set
			{
				_angle = value % 360;
				_force = (Vector2.right.Rotate(_angle) * _strength).Round(0.0001F);
				_direction = _force.normalized;
				_hasChanged = true;
			}
		}

		[SerializeField, PropertyField(typeof(MinAttribute))]
		float _strength = 20;
		public float Strength
		{
			get { return _strength; }
			set
			{
				_strength = value;
				_force = (Vector2.right.Rotate(_angle) * _strength).Round(0.0001F);
				_direction = _force.normalized;
				_hasChanged = true;
			}
		}

		[SerializeField, PropertyField]
		Vector2 _direction = new Vector2(0, -1);
		public Vector2 Direction
		{
			get { return _direction; }
			set
			{
				_direction = value.normalized.Round(0.0001F);
				_force = _direction * _strength;
				_angle = _direction.Angle();
				_hasChanged = true;
			}
		}

		[SerializeField, PropertyField]
		Vector2 _force = new Vector2(0, -20);
		public Vector2 Force
		{
			get { return _force; }
			set
			{
				_force = value;
				_strength = _force.magnitude;
				_direction = _force.normalized;
				_angle = _direction.Angle();
				_hasChanged = true;
			}
		}

		Vector2 _left;
		public Vector2 Left
		{
			get
			{
				if (_hasChanged)
					UpdateForces();

				return _right;
			}
		}

		Vector2 _right;
		public Vector2 Right
		{
			get
			{
				if (_hasChanged)
					UpdateForces();

				return _right;
			}
		}

		bool _hasChanged = true;

		void UpdateForces()
		{
			_left = _force.Rotate(90).normalized;
			_right = -_left;

			_hasChanged = false;
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