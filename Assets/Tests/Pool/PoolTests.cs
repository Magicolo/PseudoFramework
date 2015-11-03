using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo.Internal.Tests
{
	public class PoolTests : PMonoBehaviour
	{
		public CircleZone Zone;
		public PMonoBehaviour[] Prefabs;
		public PMonoBehaviour Poolable;

		[Button]
		public bool createRandomItemInPrefabs;
		void CreateRandomItemInPrefabsz()
		{
			PMonoBehaviour item = Pools.BehaviourPool.Create(Prefabs.GetRandom(), Zone.GetRandomWorldPoint(), CachedTransform);
			StartCoroutine(RecycleAfterDelay(item, 1f));
		}

		[Button]
		public bool create1000RandomItemInPrefabs;
		void Create1000RandomItemInPrefabsz()
		{
			for (int i = 0; i < 1000; i++)
				CreateRandomItemInPrefabsz();
		}

		[Button]
		public bool instantiate1000RandomItemInPrefabs;
		void Instantiate1000RandomItemInPrefabsz()
		{
			for (int i = 0; i < 1000; i++)
			{
				PMonoBehaviour item = Instantiate(Prefabs.GetRandom());
				item.transform.parent = transform;
				StartCoroutine(DestroyAfterDelay(item, 1f));
			}
		}

		[Button]
		public bool createPoolableItem;
		void CreatePoolableItemz()
		{
			PMonoBehaviour item = Pools.BehaviourPool.Create(Poolable);
			StartCoroutine(RecycleAfterDelay(item, 1f));
		}

		void Update()
		{
			if (createRandomItemInPrefabs)
				CreateRandomItemInPrefabsz();
			else if (create1000RandomItemInPrefabs)
				Create1000RandomItemInPrefabsz();
			else if (instantiate1000RandomItemInPrefabs)
				Instantiate1000RandomItemInPrefabsz();
			else if (createPoolableItem)
				CreatePoolableItemz();
		}

		IEnumerator RecycleAfterDelay(PMonoBehaviour item, float delay)
		{
			for (float counter = 0f; counter < delay; counter += Time.deltaTime)
				yield return null;

			Pools.BehaviourPool.Recycle(item);
		}

		IEnumerator DestroyAfterDelay(PMonoBehaviour item, float delay)
		{
			for (float counter = 0f; counter < delay; counter += Time.deltaTime)
				yield return null;

			item.gameObject.Destroy();
		}
	}
}