using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pseudo.Internal.Entity3
{
	public class SystemBase : ISystem
	{
		public ISystemManager SystemManager { get { return systemManager; } }
		public IEntityManager EntityManager { get { return entityManager; } }

		ISystemManager systemManager;
		IEntityManager entityManager;

		public SystemBase(ISystemManager systemManager, IEntityManager entityManager)
		{
			this.systemManager = systemManager;
			this.entityManager = entityManager;
		}
	}
}
