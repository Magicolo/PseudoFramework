using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal.Injection
{
	public class TransientGameObjectFactory : IFactory<GameObject>
	{
		readonly GameObject prefab;
		readonly IInjector injector;

		public TransientGameObjectFactory(GameObject prefab, IInjector injector)
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
			var instance = UnityEngine.Object.Instantiate(prefab);
			injector.Inject(instance, true);

			return instance;
		}
	}
}
