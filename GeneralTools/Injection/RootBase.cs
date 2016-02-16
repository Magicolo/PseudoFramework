using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using UnityEngine.SceneManagement;

namespace Pseudo.Internal.Injection
{
	public abstract class RootBase : MonoBehaviour
	{
		public BindingInstaller[] Installers = new BindingInstaller[0];

		public IBinder Binder
		{
			get { return binder; }
		}

		protected IBinder binder;

		void Awake()
		{
			Initialize();
		}

		void Start()
		{
			InjectAll();
		}

		protected virtual void Initialize()
		{
			binder = CreateBinder();

			for (int i = 0; i < Installers.Length; i++)
				binder.Install(Installers[i]);
		}

		protected virtual void InjectAll()
		{
			var roots = SceneManager.GetActiveScene().GetRootGameObjects();

			for (int i = 0; i < roots.Length; i++)
				Binder.Injector.Inject(roots[i], true);
		}

		protected abstract IBinder CreateBinder();
	}
}
