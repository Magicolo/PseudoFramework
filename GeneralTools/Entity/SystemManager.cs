﻿using Pseudo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Zenject;

namespace Pseudo
{
	public class SystemManager : ISystemManager, ITickable, IFixedTickable, ILateTickable
	{
		public event Action<ISystem> OnSystemAdded;
		public event Action<ISystem> OnSystemRemoved;
		public IList<ISystem> Systems
		{
			get { return readonlySystems; }
		}

		readonly ITimeChannel timeChannel;
		readonly List<ISystem> systems;
		readonly IList<ISystem> readonlySystems;
		readonly List<IUpdateable> updateables;
		readonly List<float> updateCounters;
		readonly List<ILateUpdateable> lateUpdateables;
		readonly List<float> lateUpdateCounters;
		readonly List<IFixedUpdateable> fixedUpdateables;

		[Inject]
		DiContainer container = null;

		[Inject]
		public SystemManager() : this(TimeManager.Unity) { }

		public SystemManager(ITimeChannel timeChannel)
		{
			this.timeChannel = timeChannel;
			systems = new List<ISystem>();
			readonlySystems = systems.AsReadOnly();
			updateables = new List<IUpdateable>();
			updateCounters = new List<float>();
			lateUpdateables = new List<ILateUpdateable>();
			lateUpdateCounters = new List<float>();
			fixedUpdateables = new List<IFixedUpdateable>();
		}

		/// <summary>
		/// Registers an ISystem instance to the SystemManager.
		/// </summary>
		/// <param name="system">The ISystem instance to register.</param>
		public virtual void AddSystem(ISystem system)
		{
			systems.Add(system);
			system.Active = true;

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

			if (OnSystemAdded != null)
				OnSystemAdded(system);
		}

		/// <summary>
		/// Registers an ISystem instance of the provided type to the SystemManager.
		/// </summary>
		/// <typeparam name="T">The type of the ISystem instance.</typeparam>
		public virtual void AddSystem<T>() where T : class, ISystem
		{
			AddSystem(container.Instantiate<T>());
		}

		/// <summary>
		/// Unregisters an ISystem instance from the SystemManager.
		/// </summary>
		/// <param name="system">The ISystem instance to unregister.</param>
		public virtual void RemoveSystem(ISystem system)
		{
			if (systems.Remove(system))
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

				if (OnSystemRemoved != null)
					OnSystemRemoved(system);
			}
		}

		/// <summary>
		/// Unregisters all ISystem instances from the SystemManager.
		/// </summary>
		public virtual void RemoveAllSystems()
		{
			for (int i = 0; i < systems.Count; i++)
			{
				var system = systems[i];

				if (OnSystemRemoved != null)
					OnSystemRemoved(system);
			}

			systems.Clear();
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
