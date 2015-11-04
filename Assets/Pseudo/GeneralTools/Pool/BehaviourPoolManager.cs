using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal;

namespace Pseudo
{
	public class BehaviourPoolManager<T> : ComponentPoolManager<T> where T : MonoBehaviour, IPoolable
	{
		public TC CreateCopy<TC>(TC reference) where TC : class, T, ICopyable<TC>
		{
			PrefabPool<T> pool = GetPool(reference);
			TC item = pool.CreateCopy(reference);

			return item;
		}

		public void CreateElements<TC>(IList<TC> array) where TC : class, T, ICopyable<TC>
		{
			if (array == null)
				return;

			for (int i = 0; i < array.Count; i++)
				array[i] = CreateCopy(array[i]);
		}

		protected override PrefabPool<T> CreatePool(T identifier)
		{
			BehaviourPool<T> pool = new BehaviourPool<T>(identifier);
			pool.Transform.parent = cachedTransform.Value;

			return pool;
		}
	}
}