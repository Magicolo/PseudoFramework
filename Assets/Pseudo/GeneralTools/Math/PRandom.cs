using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

public static class PRandom
{
	public static readonly Random Generator = new Random(Environment.TickCount);

	static List<float> weightSums = new List<float>();

	public static int Range(int min, int max)
	{
		return (int)Math.Round(Range((double)min, (double)max, ProbabilityDistributions.Uniform));
	}

	public static int Range(int min, int max, ProbabilityDistributions distribution)
	{
		return (int)Math.Round(Range((double)min, (double)max, distribution));
	}

	public static float Range(float min, float max)
	{
		return (float)Range((double)min, (double)max, ProbabilityDistributions.Uniform);
	}

	public static float Range(float min, float max, ProbabilityDistributions distribution)
	{
		return (float)Range((double)min, (double)max, distribution);
	}

	public static double Range(double min, double max)
	{
		return Range(min, max, ProbabilityDistributions.Uniform);
	}

	public static double Range(double min, double max, ProbabilityDistributions distribution)
	{
		double randomValue = 0d;

		switch (distribution)
		{
			default:
				randomValue = Generator.NextDouble();
				return PMath.Clamp(randomValue * (max - min) + min, min, max);
			case ProbabilityDistributions.Proportional:
				return PMath.Clamp(Math.Pow(2d, Range(Math.Log(min, 2d), Math.Log(max, 2d))), min, max);
			case ProbabilityDistributions.Normal:
				while (true)
				{
					double value1 = Range(-1d, 1d);
					double value2 = Range(-1d, 1d);
					double w = value1 * value1 + value2 * value2;

					if (w != 0d && w <= 1d)
					{
						double y = Math.Sqrt(-2d * Math.Log(w) / w) * 0.1d;
						randomValue = value1 * y + 0.5d;
						break;
					}
				}

				return PMath.Clamp(randomValue * (max - min) + min, min, max);
		}
	}

	public static T WeightedRandom<T>(Dictionary<T, float> objectsAndWeights, ProbabilityDistributions distribution = ProbabilityDistributions.Uniform)
	{
		T[] objects = new T[objectsAndWeights.Keys.Count];
		float[] weights = new float[objectsAndWeights.Values.Count];

		objectsAndWeights.GetOrderedKeysValues(out objects, out weights);

		return WeightedRandom(objects, weights, distribution);
	}

	public static T WeightedRandom<T>(IList<T> objects, IList<float> weights, ProbabilityDistributions distribution = ProbabilityDistributions.Uniform)
	{
		weightSums.Clear();
		float weightSum = 0f;
		float randomValue = 0f;

		for (int i = 0; i < weights.Count; i++)
		{
			weightSum += weights[i];
			weightSums.Add(weightSum);
		}

		randomValue = Range(0f, weightSum, distribution);

		for (int i = 0; i < weights.Count; i++)
		{
			if (randomValue < weightSums[i])
				return objects[i];
		}

		return default(T);
	}

	public static UnityEngine.AnimationCurve DistributionToCurve(ProbabilityDistributions distribution, int definition)
	{
		UnityEngine.Keyframe[] keys = new UnityEngine.Keyframe[definition];

		for (int i = 0; i < keys.Length; i++)
			keys[i] = new UnityEngine.Keyframe((float)i / keys.Length, 0f);

		for (int i = 0; i < keys.Length * 100; i++)
		{
			int index = (int)Math.Floor((Range(1d, 10d, distribution) - 1d) / 9d * keys.Length);
			keys[index].value += 1f / definition;
		}

		return new UnityEngine.AnimationCurve(keys);
	}
}
