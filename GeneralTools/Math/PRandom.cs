using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

public static class PRandom
{
	public static readonly Random Generator = new Random(Environment.TickCount);

	static readonly List<float> weightSums = new List<float>();

	public static double NextUniform()
	{
		return Generator.NextDouble();
	}

	public static double NextExponential()
	{
		return Generator.NextDouble() * Generator.NextDouble();
	}

	public static double NextGaussian()
	{
		double value1;
		double value2;
		double sigma;

		do
		{
			value1 = Generator.NextDouble() * 2f - 1f;
			value2 = Generator.NextDouble() * 2f - 1f;
			sigma = value1 * value1 + value2 * value2;
		}
		while (sigma == 0d || sigma >= 1d);

		sigma = Math.Sqrt(-2d * Math.Log(sigma) / sigma) * 0.1d;

		return PMath.Clamp(value1 * sigma + 0.5d, 0d, 1d);
	}

	/// <summary>
	/// Random value between <paramref name="min"/> and <paramref name="max"/> inclusive.
	/// </summary>
	/// <param name="min">Lower bound.</param>
	/// <param name="max">Upper bound.</param>
	/// <returns>The random value.</returns>
	public static int Range(int min, int max)
	{
		return (int)Math.Round(Range((double)min, (double)max, ProbabilityDistributions.Uniform));
	}

	/// <summary>
	/// Random value between <paramref name="min"/> and <paramref name="max"/> inclusive.
	/// </summary>
	/// <param name="min">Lower bound.</param>
	/// <param name="max">Upper bound.</param>
	/// <param name="distribution">Distribution of probabilities.</param>
	/// <returns>The random value.</returns>
	public static int Range(int min, int max, ProbabilityDistributions distribution)
	{
		return (int)Math.Round(Range((double)min, (double)max, distribution));
	}

	/// <summary>
	/// Random value between <paramref name="min"/> and <paramref name="max"/> inclusive.
	/// </summary>
	/// <param name="min">Lower bound.</param>
	/// <param name="max">Upper bound.</param>
	/// <returns>The random value.</returns>
	public static float Range(float min, float max)
	{
		return (float)Range((double)min, (double)max, ProbabilityDistributions.Uniform);
	}

	/// <summary>
	/// Random value between <paramref name="min"/> and <paramref name="max"/> inclusive.
	/// </summary>
	/// <param name="min">Lower bound.</param>
	/// <param name="max">Upper bound.</param>
	/// <param name="distribution">Distribution of probabilities.</param>
	/// <returns>The random value.</returns>
	public static float Range(float min, float max, ProbabilityDistributions distribution)
	{
		return (float)Range((double)min, (double)max, distribution);
	}

	/// <summary>
	/// Random value between <paramref name="min"/> and <paramref name="max"/> inclusive.
	/// </summary>
	/// <param name="min">Lower bound.</param>
	/// <param name="max">Upper bound.</param>
	/// <returns>The random value.</returns>
	public static double Range(double min, double max)
	{
		return Range(min, max, ProbabilityDistributions.Uniform);
	}

	/// <summary>
	/// Random value between <paramref name="min"/> and <paramref name="max"/> inclusive.
	/// </summary>
	/// <param name="min">Lower bound.</param>
	/// <param name="max">Upper bound.</param>
	/// <param name="distribution">Distribution of probabilities.</param>
	/// <returns>The random value.</returns>
	public static double Range(double min, double max, ProbabilityDistributions distribution)
	{
		double randomValue = 0d;

		switch (distribution)
		{
			default:
				randomValue = NextUniform();
				break;
			case ProbabilityDistributions.Exponential:
				randomValue = NextExponential();
				break;
			case ProbabilityDistributions.Gaussian:
				randomValue = NextGaussian();
				break;
		}

		return PMath.Clamp(randomValue * (max - min) + min, min, max);
	}

	public static T WeightedRandom<T>(Dictionary<T, float> objectsAndWeights, ProbabilityDistributions distribution = ProbabilityDistributions.Uniform)
	{
		var objects = new T[objectsAndWeights.Keys.Count];
		var weights = new float[objectsAndWeights.Values.Count];
		objectsAndWeights.GetOrderedKeysValues(out objects, out weights);

		return WeightedRandom(objects, weights, distribution);
	}

	public static T WeightedRandom<T>(IList<T> objects, IList<float> weights, ProbabilityDistributions distribution = ProbabilityDistributions.Uniform)
	{
		float weightSum = 0f;
		float randomValue = 0f;
		var randomObject = default(T);

		for (int i = 0; i < weights.Count; i++)
		{
			weightSum += weights[i];
			weightSums.Add(weightSum);
		}

		randomValue = Range(0f, weightSum, distribution);

		for (int i = 0; i < weights.Count; i++)
		{
			if (randomValue < weightSums[i])
			{
				randomObject = objects[i];
				break;
			}
		}

		weightSums.Clear();
		return randomObject;
	}

	public static UnityEngine.AnimationCurve ToCurve(ProbabilityDistributions distribution, int definition)
	{
		var keys = new UnityEngine.Keyframe[definition];

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
