using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using UnityEngine.SceneManagement;

namespace Pseudo.Injection.Internal
{
	public abstract class RootBehaviourBase : MonoBehaviour, IRoot, ISerializationCallbackReceiver
	{
		[SerializeField]
		InstallerBehaviourBase[] installers;
		[SerializeField]
		PAssembly[] assemblies;

		public IContainer Container
		{
			get { return container; }
		}
		public List<IBindingInstaller> Installers
		{
			get { return allInstallers; }
		}

		IContainer container;
		List<IBindingInstaller> allInstallers = new List<IBindingInstaller>();

		public virtual void InstallAll()
		{
			for (int i = 0; i < assemblies.Length; i++)
				container.Binder.Bind(assemblies[i]);

			for (int i = 0; i < installers.Length; i++)
				installers[i].Install(container);

			for (int i = 0; i < allInstallers.Count; i++)
				allInstallers[i].Install(container);
		}

		protected virtual void Awake()
		{
			container = CreateContainer();
			container.Binder.Bind(GetType(), typeof(IRoot)).ToInstance(this);

			InstallAll();
		}

		protected virtual void Inject(MonoBehaviour[] injectables)
		{
			for (int i = 0; i < injectables.Length; i++)
				container.Injector.Inject(new InjectionContext { Container = container, Instance = injectables[i] });
		}

		public abstract void InjectAll();
		protected abstract IContainer CreateContainer();

		void ISerializationCallbackReceiver.OnBeforeSerialize() { }

		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			allInstallers = new List<IBindingInstaller>(installers);
		}
	}
}
