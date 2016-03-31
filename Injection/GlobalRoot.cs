using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Injection.Internal;

namespace Pseudo.Injection
{
	public class GlobalRoot : RootBehaviourBase
	{
		public override void InjectAll()
		{
			Inject(FindObjectsOfType<MonoBehaviour>());
		}

		protected override IBinder CreateBinder()
		{
			return new Binder();
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
