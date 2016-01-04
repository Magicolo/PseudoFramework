using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo.Internal.Pool
{
	public abstract class PrefabPool : Pool
	{
		protected PrefabPool(UnityEngine.Object reference, int startSize = 4) : base(reference, reference.GetType(), startSize) { }

		public override void Clear()
		{
			lock (instances)
			{
				while (instances.Count > 0)
				{
					var instance = (UnityEngine.Object)instances.Dequeue();

					if (instance != null)
						instance.Destroy();
				}
			}

			lock (toInitialize)
			{
				while (toInitialize.Count > 0)
				{
					var instance = (UnityEngine.Object)toInitialize.Dequeue();

					if (instance != null)
						instance.Destroy();
				}
			}
		}

		protected override object CreateInstance()
		{
			var instance = UnityEngine.Object.Instantiate((UnityEngine.Object)reference);

			if (isInitializable)
				((IPoolInitializable)instance).OnPrePoolInitialize();

			PoolUtility.InitializeFields(instance, setters);

			if (isInitializable)
				((IPoolInitializable)instance).OnPostPoolInitialize();

			return instance;
		}
	}
}