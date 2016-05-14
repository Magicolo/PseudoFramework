using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Injection.Internal;

namespace Pseudo.Injection
{
	[ExecutionOrder(-9999)]
	public class GlobalRoot : RootBehaviourBase
	{
		public override void InjectAll()
		{
			Inject(FindObjectsOfType<MonoBehaviour>());
		}

		protected override IContainer CreateContainer()
		{
			return new Container();
		}

		protected override void Awake()
		{
			base.Awake();

			DontDestroyOnLoad(gameObject);
		}
	}
}
