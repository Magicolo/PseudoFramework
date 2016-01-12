using Pseudo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Pseudo.Internal.Entity3
{
	public class SystemManager : ISystemManager
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

		/// <summary>
		/// Method that will update all registered IUpdateable ISystem instances.
		/// </summary>
		public virtual void Update()
		{
			for (int i = 0; i < updateables.Count; i++)
			{
				var updateable = updateables[i];

				if (updateable.Active)
				{
					float updateCounter = (updateCounters[i] += timeChannel.DeltaTime);

					if (updateCounter >= updateable.UpdateRate)
					{
						updateCounters[i] -= updateable.UpdateRate;
						updateable.Update();
					}
				}
			}
		}

		/// <summary>
		/// Method that will update all registered ILateUpdateable ISystem instances.
		/// </summary>
		public virtual void LateUpdate()
		{
			for (int i = 0; i < lateUpdateables.Count; i++)
			{
				var lateUpdateable = lateUpdateables[i];

				if (lateUpdateable.Active)
				{
					float lateUpdateCounter = (lateUpdateCounters[i] += timeChannel.DeltaTime);

					if (lateUpdateCounter >= lateUpdateable.LateUpdateRate)
					{
						lateUpdateCounters[i] -= lateUpdateable.LateUpdateRate;
						lateUpdateable.LateUpdate();
					}
				}
			}
		}

		/// <summary>
		/// Method that will update all registered IFixedUpdateable ISystem instances.
		/// </summary>
		public virtual void FixedUpdate()
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
