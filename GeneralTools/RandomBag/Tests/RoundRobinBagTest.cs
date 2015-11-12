using UnityEngine;
using System.Collections.Generic;
using Pseudo;

public class RoundRobinBagTest : MonoBehaviour
{

	public bool test;

	void OnDrawGizmos()
	{
		if (test)
		{
			test = false;
			//testSimpleRoundRobin();
			//testSuffleOnEnd();
			//testSuffleOnAdd();
		}
	}

	/*void testSimpleRoundRobin()
	{
		RoundRobinBag<int> bag = new RoundRobinBag<int>();
		bag.add(1);
		bag.add(2);
		bag.add(3);
		bag.add(4);
		int[] anwers = { 1, 2, 3, 4, 1, 2, 3, 4 };
		testBag(bag, anwers);
	}


	void testSuffleOnEnd()
	{
		Debug.Log("Doing " + System.Reflection.MethodBase.GetCurrentMethod().Name);
		RoundRobinBag<int> bag = new RoundRobinBag<int>(RoundRobinBagOptions.SUFFLE_ON_END);
		bag.add(1);
		bag.add(2);
		bag.add(3);
		bag.add(4);

		for (int i = 1; i <= 4; i++)
		{
			int next = bag.next();
			Debug.Log("First Pass " + i + " == " + next + " : " + (i == next));
		}

		Debug.Log("Should have Reset");
		for (int i = 1; i <= 4; i++)
		{
			int next = bag.next();
			Debug.Log("Second Pass " + i + " ?? " + next);
		}
	}

	void testSuffleOnAdd()
	{
		Debug.Log("Doing " + System.Reflection.MethodBase.GetCurrentMethod().Name);
		RoundRobinBag<int> bag = new RoundRobinBag<int>(RoundRobinBagOptions.SUFFLE_ON_ADD);
		bag.add(1);
		bag.add(2);
		bag.add(3);

		Debug.Log("Should have Reset");
		for (int i = 1; i <= 3; i++)
		{
			int next = bag.next();
			Debug.Log("First Pass " + next + " == " + i + " : " + (i == next));
		}

		bag.add(4);

		Debug.Log("Should have Reset");

		for (int i = 1; i <= 4; i++)
		{
			int next = bag.next();
			Debug.Log("Second Pass " + next + " ?? " + i);
		}
	}


	void testBag(Bag<int> bag, int[] answers)
	{
		foreach (var answer in answers)
		{
			int next = bag.next();
			if (next != answer)
			{
				Debug.LogError("ERROR with answer " + next + " != " + answer);
			}
		}
		Debug.Log("Passed " + System.Reflection.MethodBase.GetCurrentMethod().Name);
	}*/
}
