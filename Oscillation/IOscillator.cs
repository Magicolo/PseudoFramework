using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Pseudo.Oscillation
{
	public enum WaveShapes
	{
		Sine,
		Triangle,
		Square,
		InQuad,
		OutQuad,
		InOutQuad,
		OutInQuad,
		InCubic,
		OutCubic,
		InOutCubic,
		OutInCubic,
		SmoothStep,
		WhiteNoise,
		PerlinNoise,
	}

	public interface IOscillator
	{
		void Oscillate(object target, OscillationSettings[] settings, int flags, float time);
	}
}
