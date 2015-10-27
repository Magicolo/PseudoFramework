using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

[Serializable]
public struct MinMax
{
	[SerializeField]
	float min;

	[SerializeField]
	float max;

	public float Min { get { return min; } set { min = value; } }
	public float Max { get { return max; } set { max = value; } }

	public MinMax(float min, float max)
	{
		this.min = min;
		this.max = max;
	}

	public float GetRandom(ProbabilityDistributions distribution)
	{
		return PRandom.Range(min, max, distribution);
	}
}
