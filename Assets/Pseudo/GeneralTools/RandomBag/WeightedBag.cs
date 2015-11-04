using System;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo
{
	public class WeightedBag<V> : Bag<V>
	{

		Random random;

		float totalWeight;
		List<PrandomBagWeightValue<V>> bag = new List<PrandomBagWeightValue<V>>();

		[FlagsAttribute]
		enum PrandomBagOptions { }

		public WeightedBag(Random random)
		{
			this.random = random;
		}


		public void Add(float MaxWeight, float PickedWeightRemoveAmount, float WeightRegenerationAmount, float CurrentWeight, V value)
		{
			bag.Add(new PrandomBagWeightValue<V>(MaxWeight, PickedWeightRemoveAmount, WeightRegenerationAmount, value));
			totalWeight += MaxWeight;
		}
		public void Add(float MaxWeight, float PickedWeightRemoveAmount, float WeightRegenerationAmount, V value)
		{
			Add(MaxWeight, PickedWeightRemoveAmount, WeightRegenerationAmount, value);
		}

		public V Next()
		{
			float weightOfNext = (float)(random.NextDouble() * totalWeight);
			float cumulativeWeight = 0;
			int indexChoosen = -1;
			bool found = false;
			for (int i = 0; i < bag.Count; i++)
			{
				var item = bag[i];
				cumulativeWeight += item.CurrentWeight;
				if (found || weightOfNext > cumulativeWeight)
				{
					item.CurrentWeight += item.WeightRegenerationAmount;
					item.CurrentWeight = Math.Max(item.CurrentWeight, item.CurrentWeight);
				}
				else
				{
					indexChoosen = i;
					found = true;
				}
			}

			if (found)
				return bag[indexChoosen].Value;
			else
				return bag.First().Value;
		}

		public void Reset()
		{
			for (int i = 0; i < bag.Count; i++)
			{
				bag[i].CurrentWeight = bag[i].MaxWeight;
			}
		}
	}

	public class PrandomBagWeightValue<V>
	{
		public float CurrentWeight;
		public float MaxWeight;
		public float PickedWeightRemoveAmount;
		public float WeightRegenerationAmount;
		public V Value;

		public PrandomBagWeightValue(float maxWeight, float pickedWeightRemoveAmount, float weightRegenerationAmount, V value)
		{
			MaxWeight = maxWeight;
			PickedWeightRemoveAmount = pickedWeightRemoveAmount;
			WeightRegenerationAmount = weightRegenerationAmount;
			this.Value = value;
		}
	}
}
