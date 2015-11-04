using System;
using System.Collections.Generic;
using System.Linq;

namespace Pseudo
{
	public class WeightedBag<V> : Bag<V>
	{

		Random random;

		float totalWeight;
		List<WeightedBagWeightValue<V>> bag = new List<WeightedBagWeightValue<V>>();

		public WeightedBag(Random random)
		{
			this.random = random;
		}

		public void Add(float weight, V value)
		{
			bag.Add(new WeightedBagWeightValue<V>(totalWeight + weight, value));
			totalWeight += weight;
		}

		public V Next()
		{
			float randomNumber = (float)(random.NextDouble() * totalWeight);
			foreach (var item in bag)
			{
				if (randomNumber < item.Weight)
				{
					return item.Value;
				}
			}
			UnityEngine.Debug.LogError("WeightedBag pas trouvé de chiffre... BUG!? !");
			return bag.First().Value;
		}

		public void Reset()
		{

		}
	}

	public struct WeightedBagWeightValue<V>
	{
		public float Weight;
		public V Value;

		public WeightedBagWeightValue(float weight, V value) : this()
		{
			this.Weight = weight;
			this.Value = value;
		}
	}
}
