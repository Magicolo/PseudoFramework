using UnityEngine;
using System.Collections;
using System;
using Pseudo;
using Pseudo.Injection;
using Pseudo.EntityFramework;
using Pseudo.Communication;

namespace Pseudo
{
	public enum Camera
	{
		Main,
		UI
	}

	public class SceneInstaller : InstallerBehaviourBase
	{
		public Camera MainCamera;
		public Camera UICamera;

		public override void Install(IContainer container)
		{
			container.Binder.Bind<EntityManager, IEntityManager>().ToSelf().AsSingleton();
			container.Binder.Bind<Messager, IMessager>().ToSelf().AsSingleton();
			container.Binder.Bind<Camera>().ToInstance(MainCamera).WhenHas(Camera.Main);
			container.Binder.Bind<Camera>().ToInstance(UICamera).WhenHas(Camera.UI);
		}
	}
}