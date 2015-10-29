using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo.Internal.Tests
{
	public class PoolManagerTests : PMonoBehaviour
	{
		public UnityEngine.Object[] Prefabs;

		[Button]
		public bool createRandomItemInPrefabs;
		void CreateRandomItemInPrefabs()
		{
			UnityEngine.Object item = PoolManager.Instance.Create(Prefabs.GetRandom());
			StartCoroutine(RecycleAfterDelay(item, 3f));
		}

		IEnumerator RecycleAfterDelay(UnityEngine.Object item, float delay)
		{
			for (float counter = 0f; counter < delay; counter += Time.deltaTime)
				yield return null;

			PoolManager.Instance.Recycle(item);
		}
	}
}