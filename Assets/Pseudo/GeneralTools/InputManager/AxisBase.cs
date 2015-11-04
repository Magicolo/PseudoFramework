using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo.Internal.Input
{
	public abstract class AxisBase
	{
		protected bool axisJustDown;
		protected bool axisJustUp;
		protected bool axisDown;

		protected abstract string AxisName { get; }
		public abstract float Threshold { get; set; }

		public float GetValue()
		{
			float value = UnityEngine.Input.GetAxisRaw(AxisName);
			value = Mathf.Abs(value) >= Threshold ? value : 0f;

			axisJustDown = !axisDown && value != 0f;
			axisJustUp = axisDown && value == 0f;
			axisDown = value != 0f;

			return value;
		}

		public bool GetAxisDown()
		{
			GetValue();

			return axisJustDown;
		}

		public bool GetAxisUp()
		{
			GetValue();

			return axisJustUp;
		}

		public bool GetAxis()
		{
			GetValue();

			return axisDown;
		}
	}
}