using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

public static class PRandom
{
	public static readonly Random RandomGenerator = new Random(Environment.TickCount);

	public static double RandomRange(double min, double max, ProbabilityDistributions distribution = ProbabilityDistributions.Uniform)
	{
		double randomValue = 0d;

		switch (distribution)
		{
			default:
				randomValue = RandomGenerator.NextDouble();
				return PMath.Clamp(randomValue * (max - min) + min, min, max);
			case ProbabilityDistributions.Normal:
				while (true)
				{
					double value1 = 2d * RandomGenerator.NextDouble() - 1d;
					double value2 = 2d * RandomGenerator.NextDouble() - 1d;
					double w = value1 * value1 + value2 * value2;

					if (w <= 1)
					{
						double y = Math.Sqrt(-2d * Math.Log(w) / w) * 0.125d;
						randomValue = value1 * y + 0.5f;
						break;
					}
				}

				return PMath.Clamp(randomValue * (max - min) + min, min, max);
			case ProbabilityDistributions.Proportional:
				return Math.Pow(2f, RandomRange(Math.Log(min, 2d), Math.Log(max, 2d)));
		}
	}

	public static int RandomRange(int min, int max, ProbabilityDistributions distribution = ProbabilityDistributions.Uniform)
	{
		return (int)Math.Round(RandomRange((double)min, (double)max, distribution));
	}

	public static float RandomRange(float min, float max, ProbabilityDistributions distribution = ProbabilityDistributions.Uniform)
	{
		return (float)RandomRange((double)min, (double)max, distribution);
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
		float[] weightSums = new float[weights.Count];
		float weightSum = 0f;
		float randomValue = 0f;

		for (int i = 0; i < weightSums.Length; i++)
		{
			weightSum += weights[i];
			weightSums[i] = weightSum;
		}

		randomValue = RandomRange(0f, weightSum, distribution);

		for (int i = 0; i < weightSums.Length; i++)
		{
			if (randomValue < weightSums[i])
				return objects[i];
		}

		return default(T);
	}
}
