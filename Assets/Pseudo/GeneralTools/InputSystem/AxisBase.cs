using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo.Internal
{
	public class AxisBase
	{
		[SerializeField]
		protected string _name = "";
		public string Name { get { return _name; } }

		[SerializeField]
		protected string _axis;
		public virtual string Axis
		{
			get { return _axis; }
			set
			{
				_axis = value;
				_lastValue = 0;
			}
		}

		protected float _lastValue;
		public float LastValue { get { return _lastValue; } set { _lastValue = value; } }

		public AxisBase(string name, string axis)
		{
			_name = name;
			_axis = axis;
		}

		public float GetValue()
		{
			return Input.GetAxis(_axis);
		}
	}
}