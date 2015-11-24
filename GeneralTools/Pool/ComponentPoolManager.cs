using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal;

namespace Pseudo
{
	public class ComponentPoolManager<T> : PrefabPoolManager<T> where T : Component
	{
		protected override UnityEngine.Object GetPoolKey(T identifier)
		{
			return identifier.gameObject;
		}

		protected override PrefabPoolz<T> CreatePool(T identifier)
		{
			ComponentPoolz<T> pool = new ComponentPoolz<T>(identifier);
			pool.Transform.parent = cachedTransform.Value;

			return pool;
		}
	}
}
