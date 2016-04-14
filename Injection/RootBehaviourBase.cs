using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using UnityEngine.SceneManagement;

namespace Pseudo.Injection.Internal
{
	public abstract class RootBehaviourBase : MonoBehaviour, IRoot
	{
		[SerializeField]
		BehaviourInstallerBase[] installers;
		[SerializeField]
		PAssembly[] assemblies;

		public IContainer Container
		{
			get { return container; }
		}
		public IEnumerable<IBindingInstaller> Installers
		{
			get { return installers.Concat(additionnalInstallers); }
		}

		IContainer container;
		readonly List<IBindingInstaller> additionnalInstallers = new List<IBindingInstaller>();

		public virtual void InstallAll()
		{
			for (int i = 0; i < assemblies.Length; i++)
				container.Binder.Bind(assemblies[i]);

			for (int i = 0; i < installers.Length; i++)
				installers[i].Install(container);

			for (int i = 0; i < additionnalInstallers.Count; i++)
				additionnalInstallers[i].Install(container);
		}

		public void AddInstaller(IBindingInstaller installer)
		{
			additionnalInstallers.Add(installer);
		}

		public void RemoveInstaller(IBindingInstaller installer)
		{
			additionnalInstallers.Remove(installer);
		}

		protected virtual void Awake()
		{
			container = CreateContainer();
			container.Binder.Bind(GetType(), typeof(IRoot)).ToInstance(this);

			InstallAll();
		}

		protected virtual void Inject(MonoBehaviour[] injectables)
		{
			// Pre-scene injection callback
			for (int i = 0; i < injectables.Length; i++)
			{
				var injectable = injectables[i] as IRootInjectable;

				if (injectable != null)
					injectable.OnPreRootInject(this);
			}

			// Injection
			for (int i = 0; i < injectables.Length; i++)
				container.Injector.Inject(injectables[i]);

			// Post-scene injection callback
			for (int i = 0; i < injectables.Length; i++)
			{
				var injectable = injectables[i] as IRootInjectable;

				if (injectable != null)
					injectable.OnPostRootInject(this);
			}
		}

		public abstract void InjectAll();
		protected abstract IContainer CreateContainer();
	}
}
