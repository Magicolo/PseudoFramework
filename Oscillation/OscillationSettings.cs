using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Oscillation
{
	[Serializable]
	public struct OscillationSettings
	{
		public WaveShapes WaveShape;
		[Min]
		public float Frequency;
		public float Amplitude;
		public float Center;
		[Range(0f, 100f)]
		public float Offset;
		[Range(0f, 1f)]
		public float Ratio;
	}
}
