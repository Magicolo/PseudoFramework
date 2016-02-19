using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Internal;
using System;

namespace Pseudo.Internal.Input
{
	[Serializable]
	public class KeyboardAxis : AxisBase
	{
		[SerializeField]
		protected string axis;
		[SerializeField, Min]
		protected float threshold;

		public string Axis { get { return axis; } set { axis = value; } }
		public override float Threshold { get { return threshold; } set { threshold = value; } }
		protected override string AxisName { get { return axis; } }

		public KeyboardAxis(string axis, float threshold)
		{
			this.axis = axis;
			this.threshold = threshold;
		}
	}
}