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
			container.Binder.Bind<EntityManager, IEntityManager>().ToSingleton();
			container.Binder.Bind<MessageManager, IMessageManager>().ToSingleton();
			container.Binder.Bind<Camera>().ToInstance(MainCamera).When(c => c.Identifier == "Main");
			container.Binder.Bind<Camera>().ToInstance(UICamera).When(c => c.Identifier == "UI");
		}
	}
}