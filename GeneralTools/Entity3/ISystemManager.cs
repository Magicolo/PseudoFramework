using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pseudo.Internal.Entity3
{
	public interface ISystemManager
	{
		event Action<ISystem> OnSystemAdded;
		event Action<ISystem> OnSystemRemoved;
		IList<ISystem> Systems { get; }

		void AddSystem(ISystem system);
		void RemoveSystem(ISystem system);
		void RemoveAllSystems();
	}
}
