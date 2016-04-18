using UnityEngine;
using System.Collections;
using System;
using Pseudo;
using Pseudo.Injection;
using Pseudo.EntityFramework;
using Pseudo.Communication;

namespace Pseudo
{
	public class SceneInstaller : BehaviourInstallerBase
	{
		public Camera MainCamera;
		public Camera UICamera;

		public override void Install(IContainer container)
		{
			container.Binder.Bind<EntityManager, IEntityManager>().ToSelf().AsSingleton();
			container.Binder.Bind<MessageManager, IMessageManager>().ToSelf().AsSingleton();
			container.Binder.Bind<Camera>().ToInstance(MainCamera).WhenHas("Main");
			container.Binder.Bind<Camera>().ToInstance(UICamera).WhenHas("UI");
		}
	}
}