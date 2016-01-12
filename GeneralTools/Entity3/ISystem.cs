using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pseudo.Internal.Entity3
{
	public interface ISystem
	{
		ISystemManager SystemManager { get; }
		IEntityManager EntityManager { get; }
	}
}
