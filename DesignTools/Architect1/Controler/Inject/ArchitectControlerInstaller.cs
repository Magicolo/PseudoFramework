using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Injection;
using Pseudo.EntityFramework;
using Pseudo.Communication;

namespace Pseudo.Architect
{
	[Serializable]
	public class ArchitectControlerInstaller : BehaviourInstallerBase
	{
		public override void Install(IContainer container)
		{
			PDebug.LogMethod();
			container.Binder.Bind<ArchitectControler>().ToSelf().AsSingleton();
			container.Binder.Bind<ArchitectToolControler>().ToSelf().AsSingleton();
			container.Binder.Bind<ArchitectLayerControler>().ToSelf().AsSingleton();
			container.Binder.Bind<ArchitectCameraControler>().ToSelf().AsSingleton();
			container.Binder.Bind<DrawingControler>().ToSelf().AsSingleton();
		}
	}
}
