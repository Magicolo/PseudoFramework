using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Injection.Internal;
using Pseudo.Injection;

namespace Pseudo.Architect
{
	[Serializable]
	public class ArchitectRoot : RootBehaviourBase
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

		void Reset()
		{
			this.SetExecutionOrder(-9999);
		}
	}
}
