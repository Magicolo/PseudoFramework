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
		public override void Install(IBinder binder)
		{
			binder.Bind<IEntityManager>().ToSingle<EntityManager>();
			binder.Bind<MessageManager>().ToSingle();
		}
	}
}