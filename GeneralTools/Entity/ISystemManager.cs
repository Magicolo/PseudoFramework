using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pseudo
{
	public interface ISystemManager
	{
		event Action<ISystem> OnSystemAdded;
		event Action<ISystem> OnSystemRemoved;
		IList<ISystem> Systems { get; }

		void AddSystem(ISystem system);
		void AddSystem<T>() where T : class, ISystem;
		void RemoveSystem(ISystem system);
		void RemoveAllSystems();
	}
}
