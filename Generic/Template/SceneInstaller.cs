using UnityEngine;
using System.Collections;
using Zenject;
using System;
using Pseudo;
using Pseudo.Internal;
using Pseudo.Internal.Entity;
using Pseudo.Internal.Communication;

namespace Pseudo
{
	public class SceneInstaller : MonoInstaller
	{
		public override void InstallBindings()
		{
			BindManagers();
		}

		protected virtual void BindManagers()
		{
			Container.BindAllInterfacesToSingle<EntityManager>();
			Container.Bind<MessageManager>().ToSingle();
			Container.BindAllInterfacesToSingle<EventManager>();
			Container.BindLateTickablePriority<EventManager>(100);
		}
	}
}