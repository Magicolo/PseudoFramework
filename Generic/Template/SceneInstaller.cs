using UnityEngine;
using System.Collections;
using System;
using Pseudo;
using Pseudo.Internal;
using Pseudo.Internal.Entity;
using Pseudo.Internal.Communication;

namespace Pseudo
{
	public class SceneInstaller : BindingInstaller
	{
		public Camera MainCamera;
		public Camera UICamera;

		public override void Install(IBinder binder)
		{
			binder.Bind<EntityManager, IEntityManager>().ToSingleton();
			binder.Bind<MessageManager, IMessageManager>().ToSingleton();
			binder.Bind<Camera>().ToInstance(MainCamera).When(c => c.Identifier == "Main");
			binder.Bind<Camera>().ToInstance(UICamera).When(c => c.Identifier == "UI");
		}
	}
}