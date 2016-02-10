using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo
{
	public enum WaveShapes
	{
		Sine,
		Triangle,
		Sawtooth,
		Square,
		WhiteNoise,
		PerlinNoise
	}

	public interface IOscillator
	{
		void Oscillate(object target, OscillationSettings[] settings, int flags, float time);
	}
}
