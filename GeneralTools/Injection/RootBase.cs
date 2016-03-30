using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using UnityEngine.SceneManagement;

namespace Pseudo.Internal.Injection
{
	public abstract class RootBase<TRoot> : Singleton<TRoot>, IRoot where TRoot : Singleton<TRoot>
	{
		[SerializeField]
		BindingInstaller[] installers;

		public IBinder Binder
		{
			get { return binder; }
		}
		public IEnumerable<IBindingInstaller> Installers
		{
			get { return installers.Concat(additionnalInstallers); }
		}

		IBinder binder;
		readonly List<IBindingInstaller> additionnalInstallers = new List<IBindingInstaller>();

		public virtual void InstallAll()
		{
			for (int i = 0; i < installers.Length; i++)
				installers[i].Install(binder);

			for (int i = 0; i < additionnalInstallers.Count; i++)
				additionnalInstallers[i].Install(binder);
		}

		public void AddInstaller(IBindingInstaller installer)
		{
			additionnalInstallers.Add(installer);
		}

		public void RemoveInstaller(IBindingInstaller installer)
		{
			additionnalInstallers.Remove(installer);
		}

		protected override void Awake()
		{
			base.Awake();

			binder = CreateBinder();
			binder.Bind(GetType(), typeof(IRoot)).ToInstance(this);
			InstallAll();
		}

		protected virtual void Inject(IList injectables)
		{
			// Pre-scene injection callback
			for (int i = 0; i < injectables.Count; i++)
			{
				var injectable = injectables[i] as IRootInjectable;

				if (injectable != null)
					injectable.OnPreRootInject(this);
			}

			// Injection
			for (int i = 0; i < injectables.Count; i++)
				binder.Injector.Inject(injectables[i]);

			// Post-scene injection callback
			for (int i = 0; i < injectables.Count; i++)
			{
				var injectable = injectables[i] as IRootInjectable;

				if (injectable != null)
					injectable.OnPostRootInject(this);
			}
		}

		public abstract void InjectAll();
		protected abstract IBinder CreateBinder();
	}
}
