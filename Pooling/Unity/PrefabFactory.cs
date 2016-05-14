using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Pooling.Internal
{
	public abstract class PrefabFactory<T> : FactoryBase<T> where T : UnityEngine.Object
	{
		protected readonly T prefab;

		protected PrefabFactory(T prefab)
		{
			this.prefab = prefab;
		}

		public override T Create()
		{
			return UnityEngine.Object.Instantiate(prefab);
		}
	}
}
