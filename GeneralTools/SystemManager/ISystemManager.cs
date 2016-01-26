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

		T GetSystem<T>() where T : class, ISystem;
		ISystem GetSystem(Type type);
		bool HasSystem(ISystem system);
		bool HasSystem<T>() where T : class, ISystem;
		bool HasSystem(Type type);
		void AddSystem(ISystem system);
		void AddSystem<T>() where T : class, ISystem;
		void AddSystem(Type type);
		void RemoveSystem(ISystem system);
		void RemoveSystem<T>() where T : class, ISystem;
		void RemoveSystem(Type type);
		void RemoveAllSystems();
	}
}
