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
	public class ArchitectControlerInstaller : InstallerBehaviourBase
	{
		public ArchitectBehavior ArchitectBehavior;
		public Camera MainCam;
		public Camera UICam;

		public override void Install(IContainer container)
		{
			container.Binder.Bind<ArchitectControler>().ToSelf().AsSingleton();
			container.Binder.Bind<ArchitectBehavior>().ToInstance(ArchitectBehavior);
			container.Binder.Bind<ArchitectToolControler>().ToSelf().AsSingleton();
			container.Binder.Bind<ArchitectLayerControler>().ToSelf().AsSingleton();
			container.Binder.Bind<ArchitectCameraControler>().ToSelf().AsSingleton();
			container.Binder.Bind<DrawingControler>().ToSelf().AsSingleton();
			container.Binder.Bind<Camera>().ToInstance(MainCam).WhenHas("ArchitectMain");
			container.Binder.Bind<Camera>().ToInstance(UICam).WhenHas("ArchitectUI");
		}
	}
}
