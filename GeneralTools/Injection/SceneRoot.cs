using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Internal.Injection;
using UnityEngine.SceneManagement;

namespace Pseudo
{
	public class SceneRoot : RootBase<SceneRoot>
	{
		protected override IBinder CreateBinder()
		{
			InitializeGlobalRoot();

			return new Binder(GlobalRoot.Instance == null ? null : GlobalRoot.Instance.Binder);
		}

		void Start()
		{
			InjectAll();
		}

		void Reset()
		{
			this.SetExecutionOrder(-9998);
		}

		void InitializeGlobalRoot()
		{
			if (GlobalRoot.Instance == null)
			{
				var root = Resources.Load<GlobalRoot>("GlobalRoot");

				if (root != null)
					Instantiate(root);
			}
		}
	}
}
