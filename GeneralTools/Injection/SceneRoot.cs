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
		public override void InjectAll()
		{
			Inject(SceneUtility.FindComponents<MonoBehaviour>(gameObject.scene));
		}

		protected override IBinder CreateBinder()
		{
			InitializeGlobalRoot();

			return new Binder(GlobalRoot.Instance == null ? null : GlobalRoot.Instance.Binder);
		}

		protected override void Awake()
		{
			base.Awake();

			StartCoroutine(InjectionRoutine());
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

		IEnumerator InjectionRoutine()
		{
			while (gameObject != null && !gameObject.scene.isLoaded)
				yield return null;

			InjectAll();
		}
	}
}
