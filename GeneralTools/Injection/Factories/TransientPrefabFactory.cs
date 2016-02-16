using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;

namespace Pseudo.Internal.Injection
{
	public class TransientPrefabFactory : IFactory<UnityEngine.Object>
	{
		readonly UnityEngine.Object prefab;
		readonly IInjector injector;

		public TransientPrefabFactory(UnityEngine.Object prefab, IInjector injector)
		{
			this.prefab = prefab;
			this.injector = injector;
		}

		public object Create(params object[] arguments)
		{
			return Create();
		}

		public UnityEngine.Object Create()
		{
			var instance = UnityEngine.Object.Instantiate(prefab);
			injector.Inject(instance);

			return instance;
		}
	}
}
