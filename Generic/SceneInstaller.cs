using UnityEngine;
using System.Collections;
using Zenject;
using System;
using Pseudo;

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
			Container.BindAllInterfacesToSingle<SystemManager>();
			Container.BindAllInterfacesToSingle<EventManager>();
			Container.BindLateTickablePriority<EventManager>(100);
		}
	}
}