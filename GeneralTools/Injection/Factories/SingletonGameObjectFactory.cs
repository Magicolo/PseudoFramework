using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal.Injection
{
	public class SingletonGameObjectFactory : IFactory<GameObject>
	{
		readonly GameObject prefab;
		readonly IInjector injector;
		GameObject instance;

		public SingletonGameObjectFactory(GameObject prefab, IInjector injector)
		{
			this.prefab = prefab;
			this.injector = injector;
		}

		public object Create(params object[] arguments)
		{
			return Create();
		}

		public GameObject Create()
		{
			if (instance == null)
			{
				instance = UnityEngine.Object.Instantiate(prefab);
				injector.Inject(instance, true);
			}

			return instance;
		}
	}
}
