using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo
{
	[Serializable]
	public struct Circle
	{
		[SerializeField]
		float _x;
		[SerializeField]
		float _y;
		[SerializeField]
		float _radius;

		public float XMin { get { return _x - _radius; } }
		public float XMax { get { return _x + _radius; } }
		public float YMin { get { return _y - _radius; } }
		public float YMax { get { return _y + _radius; } }
		public float Radius { get { return _radius; } set { _radius = value; } }
		public Vector2 Position { get { return new Vector2(_x, _y); } set { _x = value.x; _y = value.y; } }

		public Circle(float x, float y, float radius)
		{
			_x = x;
			_y = y;
			_radius = radius;
		}

		public Circle(Vector2 position, float radius)
		{
			_x = position.x;
			_y = position.y;
			_radius = radius;
		}

		public Circle(Circle circle)
		{
			_x = circle._x;
			_y = circle._y;
			_radius = circle._radius;
		}

		public Vector2 GetRandomPoint()
		{
			return UnityEngine.Random.insideUnitCircle * _radius + Position;
		}

		public bool Contains(Vector2 point)
		{
			return Vector2.Distance(Position, point) <= Mathf.Abs(_radius);
		}

		public bool Intersects(Circle circle)
		{
			return Vector2.Distance(Position, circle.Position) <= _radius + circle._radius;
		}

		public override string ToString()
		{
			return string.Format("Circle({0}, {1}, {2})", _x, _y, _radius);
		}
	}
}