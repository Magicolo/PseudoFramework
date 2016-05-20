using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Injection;

namespace Pseudo
{
	public partial class PMonoBehaviour
	{
		[Inject]
		IRoot root;
		[NonSerialized]
		bool injected;

		public void Inject()
		{
			root = root ?? SceneUtility.FindComponent<IRoot>(gameObject.scene);

			if (root == null || root.Container == null)
				return;

			root.Container.Injector.Inject(this);
			injected = true;
		}

		protected virtual void Start()
		{
			if (!injected)
				Inject();
		}
	}
}
