using Pseudo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace Pseudo
{
	// TODO Add System Categories (using namespace or attribute) to rapidly switch from a collection a systems to another (GameSystems, Level1Systems, DebugSystems, etc.)
	public class SystemManager : ISystemManager, ITickable, IFixedTickable, ILateTickable
	{
		public event Action<ISystem> OnSystemAdded = delegate { };
		public event Action<ISystem> OnSystemRemoved = delegate { };
		public IList<ISystem> Systems
		{
			get { return readonlySystems; }
		}

		readonly ITimeChannel timeChannel;
		readonly List<ISystem> systems;
		readonly IList<ISystem> readonlySystems;
		readonly Dictionary<Type, ISystem> typeToSystem;
		readonly List<IUpdateable> updateables;
		readonly List<float> updateCounters;
		readonly List<ILateUpdateable> lateUpdateables;
		readonly List<float> lateUpdateCounters;
		readonly List<IFixedUpdateable> fixedUpdateables;

		[Inject]
		IInstantiator container = null;

		[Inject]
		public SystemManager() : this(TimeManager.Unity) { }

		public SystemManager(ITimeChannel timeChannel)
		{
			this.timeChannel = timeChannel;
			systems = new List<ISystem>();
			readonlySystems = systems.AsReadOnly();
			typeToSystem = new Dictionary<Type, ISystem>();
			updateables = new List<IUpdateable>();
			updateCounters = new List<float>();
			lateUpdateables = new List<ILateUpdateable>();
			lateUpdateCounters = new List<float>();
			fixedUpdateables = new List<IFixedUpdateable>();
		}

		public T GetSystem<T>() where T : class, ISystem
		{
			return (T)GetSystem(typeof(T));
		}

		public ISystem GetSystem(Type type)
		{
			ISystem system;
			typeToSystem.TryGetValue(type, out system);

			return system;
		}

		public bool HasSystem(ISystem system)
		{
			return systems.Contains(system);
		}

		public bool HasSystem<T>() where T : class, ISystem
		{
			return HasSystem(typeof(T));
		}

		public bool HasSystem(Type type)
		{
			return typeToSystem.ContainsKey(type);
		}

		public void AddSystem(ISystem system, bool active = true)
		{
			Assert.IsFalse(typeToSystem.ContainsKey(system.GetType()));
			typeToSystem[system.GetType()] = system;
			systems.Add(system);

			var updateable = system as IUpdateable;

			if (updateable != null)
			{
				updateables.Add(updateable);
				updateCounters.Add(0f);
			}

			var lateUpdateable = system as ILateUpdateable;

			if (lateUpdateable != null)
			{
				lateUpdateables.Add(lateUpdateable);
				lateUpdateCounters.Add(0f);
			}

			var fixedUpdateable = system as IFixedUpdateable;

			if (fixedUpdateable != null)
				fixedUpdateables.Add(fixedUpdateable);

			system.OnInitialize();
			system.Active = active;
			OnSystemAdded(system);
		}

		public ISystem AddSystem<T>(bool active = true) where T : class, ISystem
		{
			return AddSystem(typeof(T), active);
		}

		public ISystem AddSystem(Type type, bool active = true)
		{
			Assert.IsNotNull(type);
			Assert.IsFalse(typeToSystem.ContainsKey(type));

			var system = (ISystem)container.Instantiate(type);
			AddSystem(system, active);

			return system;
		}

		public void RemoveSystem(ISystem system)
		{
			if (systems.Contains(system))
				RemoveSystem(system.GetType());
		}

		public void RemoveSystem<T>() where T : class, ISystem
		{
			RemoveSystem(typeof(T));
		}

		public void RemoveSystem(Type type)
		{
			Assert.IsNotNull(type);
			ISystem system;

			if (typeToSystem.Pop(type, out system) && systems.Remove(system))
			{
				var updateable = system as IUpdateable;
				if (updateable != null)
				{
					int index = updateables.IndexOf(updateable);

					if (index >= 0)
					{
						updateables.RemoveAt(index);
						updateCounters.RemoveAt(index);
					}
				}

				var lateUpdateable = system as ILateUpdateable;
				if (lateUpdateable != null)
				{
					int index = lateUpdateables.IndexOf(lateUpdateable);

					if (index >= 0)
					{
						lateUpdateables.RemoveAt(index);
						lateUpdateCounters.RemoveAt(index);
					}
				}

				var fixedUpdateable = system as IFixedUpdateable;
				if (fixedUpdateable != null)
					fixedUpdateables.Remove(fixedUpdateable);

				system.Active = false;
				system.OnDestroy();
				OnSystemRemoved(system);
			}
		}

		public void RemoveAllSystems()
		{
			for (int i = 0; i < systems.Count; i++)
			{
				var system = systems[i];
				OnSystemRemoved(system);
			}

			systems.Clear();
			typeToSystem.Clear();
			updateables.Clear();
			fixedUpdateables.Clear();
			lateUpdateables.Clear();
		}

		void ITickable.Tick()
		{
			for (int i = 0; i < updateables.Count; i++)
			{
				var updateable = updateables[i];

				if (updateable.Active)
				{
					float updateCounter = (updateCounters[i] += timeChannel.DeltaTime);

					if (updateCounter >= updateable.UpdateDelay)
					{
						updateCounters[i] -= updateable.UpdateDelay;
						updateable.Update();
					}
				}
			}
		}

		void ILateTickable.LateTick()
		{
			for (int i = 0; i < lateUpdateables.Count; i++)
			{
				var lateUpdateable = lateUpdateables[i];

				if (lateUpdateable.Active)
				{
					float lateUpdateCounter = (lateUpdateCounters[i] += timeChannel.DeltaTime);

					if (lateUpdateCounter >= lateUpdateable.LateUpdateDelay)
					{
						lateUpdateCounters[i] -= lateUpdateable.LateUpdateDelay;
						lateUpdateable.LateUpdate();
					}
				}
			}
		}

		void IFixedTickable.FixedTick()
		{
			for (int i = 0; i < fixedUpdateables.Count; i++)
			{
				var fixedUpdateable = fixedUpdateables[i];

				if (fixedUpdateable.Active)
					fixedUpdateable.FixedUpdate();
			}
		}
	}
}
