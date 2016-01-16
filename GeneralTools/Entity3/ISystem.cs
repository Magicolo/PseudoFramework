using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pseudo
{
	public interface ISystem
	{
		ISystemManager SystemManager { get; }
		IEntityManager EntityManager { get; }
		IEventManager EventManager { get; }
	}
}
