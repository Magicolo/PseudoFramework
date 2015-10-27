using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

public static class PMath
{
	public static double Clamp(double value, double min, double max)
	{
		return Math.Min(Math.Max(value, min), max);
	}

	public static float MidiToFrequency(float note)
	{
		return Mathf.Pow(2, (note - 69) / 12) * 440;
	}

	public static float Hypotenuse(float a)
	{
		return Hypotenuse(a, a);
	}

	public static float Hypotenuse(float a, float b)
	{
		return Mathf.Sqrt(Mathf.Pow(a, 2f) + Mathf.Pow(b, 2f));
	}
}
