using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo
{
	[Serializable]
	public struct OscillationSettings
	{
		public WaveShapes WaveShape;
		public float Frequency;
		public float Amplitude;
		public float Center;
		public float Offset;
		public float Ratio;
	}
}
