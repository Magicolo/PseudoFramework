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
		protected string name = "";
		public string Name { get { return name; } }

		[SerializeField]
		protected string axis;
		public virtual string Axis
		{
			get { return axis; }
			set
			{
				axis = value;
				lastValue = 0;
			}
		}

		protected float lastValue;
		public float LastValue { get { return lastValue; } set { lastValue = value; } }

		public AxisBase(string name, string axis)
		{
			this.name = name;
			this.axis = axis;
		}

		public float GetValue()
		{
			return Input.GetAxis(axis);
		}
	}
}