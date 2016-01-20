using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pseudo
{
	public interface ISystem
	{
		bool Active { get; set; }

		ISystemManager SystemManager { get; }
		IEntityManager EntityManager { get; }
		IEventManager EventManager { get; }

		void OnInitialize();
		void OnDestroy();
		void OnActivate();
		void OnDeactivate();
	}
}
