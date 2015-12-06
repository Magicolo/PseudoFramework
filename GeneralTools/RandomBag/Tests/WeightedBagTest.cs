using UnityEngine;
using System.Collections.Generic;
using Pseudo;

public class WeightedBagTest : MonoBehaviour
{

	public bool test;

	void OnDrawGizmos()
	{
		if (test)
		{
			test = false;
			testBase();
		}
	}

	void testBase()
	{
		SimpleWeightedBag<int> bag = new SimpleWeightedBag<int>(new System.Random());
		bag.Add(25, 1);
		bag.Add(25, 2);
		bag.Add(50, 3);
		Dictionary<int, int> valueRecieve = new Dictionary<int, int>();
		valueRecieve.Add(1, 0);
		valueRecieve.Add(2, 0);
		valueRecieve.Add(3, 0);
		for (int i = 0; i < 1000; i++)
		{
			int value = bag.Next();
			int nbTime = valueRecieve[value] + 1;
			valueRecieve.Remove(value);
			valueRecieve.Add(value, nbTime);
		}

		Debug.Log("Test base");
		foreach (var keyvalue in valueRecieve)
		{
			Debug.Log(keyvalue.Key + " : " + keyvalue.Value);
		}

	}
}