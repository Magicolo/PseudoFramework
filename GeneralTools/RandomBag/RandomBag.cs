using System;
using System.Collections.Generic;

namespace Pseudo
{
	public class RandomBag<T> : Bag<T>
	{

		Random random;

		List<T> bag;

		public RandomBag(Random random)
		{
			this.random = random;
			bag = new List<T>();
		}


		public void add(T toAdd)
		{
			bag.Add(toAdd);
		}

		public T Next()
		{
			int randomIndex = (int)(random.NextDouble() * bag.Count);
			return bag[randomIndex];
		}

		public void Reset()
		{

		}
	}
}
