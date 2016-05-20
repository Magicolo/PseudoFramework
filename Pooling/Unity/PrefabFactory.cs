using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Pooling.Internal
{
	public class PrefabFactory<T> : FactoryBase<T> where T : UnityEngine.Object
	{
		readonly T prefab;

		public PrefabFactory(T prefab)
		{
			this.prefab = prefab;
		}

		public override T Create()
		{
			return UnityEngine.Object.Instantiate(prefab);
		}
	}
}
