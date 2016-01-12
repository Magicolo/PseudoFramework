using Pseudo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pseudo.Internal.Entity3
{
	public class SystemManager : ISystemManager
	{
		public event Action<ISystem> OnSystemAdded;
		public event Action<ISystem> OnSystemRemoved;
		public IList<ISystem> AllSystems
		{
			get { return readonlySystems; }
		}

		readonly List<ISystem> allSystems;
		readonly IList<ISystem> readonlySystems;
		readonly List<IUpdateable> updateables;
		readonly List<IFixedUpdateable> fixedUpdateables;
		readonly List<ILateUpdateable> lateUpdateables;

		public SystemManager()
		{
			allSystems = new List<ISystem>();
			readonlySystems = allSystems.AsReadOnly();
			updateables = new List<IUpdateable>();
			fixedUpdateables = new List<IFixedUpdateable>();
			lateUpdateables = new List<ILateUpdateable>();
		}

		/// <summary>
		/// Registers an ISystem instance to the SystemManager.
		/// </summary>
		/// <param name="system">The ISystem instance to register.</param>
		public virtual void AddSystem(ISystem system)
		{
			allSystems.Add(system);

			var updateable = system as IUpdateable;

			if (updateable != null)
				updateables.Add(updateable);

			var fixedUpdateable = system as IFixedUpdateable;

			if (fixedUpdateable != null)
				fixedUpdateables.Add(fixedUpdateable);

			var lateUpdateable = system as ILateUpdateable;

			if (lateUpdateable != null)
				lateUpdateables.Add(lateUpdateable);

			if (OnSystemAdded != null)
				OnSystemAdded(system);
		}

		/// <summary>
		/// Unregisters an ISystem instance from the SystemManager.
		/// </summary>
		/// <param name="system">The ISystem instance to unregister.</param>
		public virtual void RemoveSystem(ISystem system)
		{
			if (allSystems.Remove(system))
			{
				var updateable = system as IUpdateable;

				if (updateable != null)
					updateables.Remove(updateable);

				var fixedUpdateable = system as IFixedUpdateable;

				if (fixedUpdateable != null)
					fixedUpdateables.Remove(fixedUpdateable);

				var lateUpdateable = system as ILateUpdateable;

				if (lateUpdateable != null)
					lateUpdateables.Remove(lateUpdateable);

				if (OnSystemRemoved != null)
					OnSystemRemoved(system);
			}
		}

		/// <summary>
		/// Unregisters all ISystem instances from the SystemManager.
		/// </summary>
		public virtual void RemoveAllSystems()
		{
			for (int i = 0; i < allSystems.Count; i++)
			{
				var system = allSystems[i];

				if (OnSystemRemoved != null)
					OnSystemRemoved(system);
			}

			allSystems.Clear();
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
					updateable.Update();
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

		/// <summary>
		/// Method that will update all registered ILateUpdateable ISystem instances.
		/// </summary>
		public virtual void LateUpdate()
		{
			for (int i = 0; i < lateUpdateables.Count; i++)
			{
				var lateUpdateable = lateUpdateables[i];

				if (lateUpdateable.Active)
					lateUpdateable.LateUpdate();
			}
		}
	}
}
