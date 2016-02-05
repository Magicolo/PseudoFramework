using UnityEngine;
using System.Collections;
using Zenject;
using System;
using Pseudo;
using Pseudo.Internal;
using Pseudo.Internal.Entity;

namespace Pseudo
{
	public class SceneInstaller : MonoInstaller
	{
		public override void InstallBindings()
		{
			BindManagers();
		}

		void BindManagers()
		{
			Container.BindAllInterfacesToSingle<EntityManager>();
			Container.BindAllInterfacesToSingle<MessageManager>();
			Container.BindAllInterfacesToSingle<EventManager>();
			Container.BindLateTickablePriority<EventManager>(100);
		}
	}
}