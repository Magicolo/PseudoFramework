using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Injection.Internal;
using UnityEngine.SceneManagement;

namespace Pseudo.Injection
{
	public class SceneRoot : RootBehaviourBase
	{
		public override void InjectAll()
		{
			Inject(SceneUtility.FindComponents<MonoBehaviour>(gameObject.scene));
		}

		protected override IContainer CreateContainer()
		{
			var root = GetOrCreateGlobalRoot();

			if (root == null)
				return new Container();
			else
				return new Container(root.Container);
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

		GlobalRoot GetOrCreateGlobalRoot()
		{
			var root = FindObjectOfType<GlobalRoot>();

			if (root == null)
			{
				var rootPrefab = Resources.Load<GlobalRoot>("GlobalRoot");

				if (rootPrefab != null)
					root = Instantiate(rootPrefab);
			}

			return root;
		}

		IEnumerator InjectionRoutine()
		{
			while (gameObject != null && !gameObject.scene.isLoaded)
				yield return null;

			InjectAll();
		}
	}
}
