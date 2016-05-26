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
	[ExecutionOrder(-9998)]
	public class SceneRoot : RootBehaviourBase
	{
		bool hasInjected;

		public override void InjectAll()
		{
			if (hasInjected || !gameObject.scene.isLoaded)
				return;

			Inject(SceneUtility.FindComponents<MonoBehaviour>(gameObject.scene));
			hasInjected = true;
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

			InjectAll();
		}

		void OnEnable()
		{
			InjectAll();
		}

		void Start()
		{
			InjectAll();
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
	}
}
