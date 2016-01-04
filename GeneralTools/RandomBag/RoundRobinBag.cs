using System;
using System.Collections.Generic;

namespace Pseudo
{
	public class RoundRobinBag<T> : Bag<T>
	{

		public enum RoundRobinBagOptions { ShuffleOnReset, ShuffleOnAdd, ShuffleOnEnd, ResetOnAdd }

		readonly List<T> bag = new List<T>();
		int currentIndex = -1;

		bool suffleOnReset;
		bool suffleOnAdd;
		bool suffleOnEnd;
		bool resetOnAdd;

		public RoundRobinBag(params RoundRobinBagOptions[] options)
		{

			foreach (var option in options)
			{
				switch (option)
				{
					case RoundRobinBagOptions.ShuffleOnAdd: suffleOnAdd = true; break;
					case RoundRobinBagOptions.ShuffleOnReset: suffleOnReset = true; break;
					case RoundRobinBagOptions.ShuffleOnEnd: suffleOnEnd = true; break;
					case RoundRobinBagOptions.ResetOnAdd: resetOnAdd = true; break;
				}
			}
		}


		public void Add(T toAdd)
		{
			bag.Add(toAdd);
			if (resetOnAdd) Reset();
			if (suffleOnAdd) Shuffle();
		}

		public T Next()
		{
			currentIndex = ++currentIndex;

			if (currentIndex >= bag.Count)
			{
				if (suffleOnEnd) Shuffle();
				currentIndex = 0;
			}


			return bag[currentIndex];
		}

		public void Reset()
		{
			currentIndex = -1;
			if (suffleOnReset) Shuffle();
		}

		void Shuffle()
		{
			bag.Shuffle();
		}
	}
}