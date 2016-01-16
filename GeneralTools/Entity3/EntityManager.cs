using Pseudo;
using Pseudo.Internal.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pseudo
{
	public class EntityManager : IEntityManager
	{
		public event Action<IEntity> OnEntityAdded;
		public event Action<IEntity> OnEntityRemoved;
		public IEntityGroup Entities
		{
			get { return entities; }
		}

		readonly EntityGroup entities = new EntityGroup();

		/// <summary>
		/// Creates a new IEntity instance and adds it to the SystemManager.
		/// </summary>
		/// <returns>The IEntity instance.</returns>
		public IEntity CreateEntity()
		{
			return CreateEntity(EntityGroups.Nothing);
		}

		/// <summary>
		/// Creates a new IEntity instance and adds it to the SystemManager.
		/// </summary>
		/// <param name="groups">The groups that the IEntity instance should be placed in.</param>
		/// <returns>The IEntity instance.</returns>
		public IEntity CreateEntity(EntityGroups groups)
		{
			var entity = new Entity(this, groups);
			AddEntity(entity);

			return entity;
		}

		/// <summary>
		/// Registers an IEntity instance to the SystemManager.
		/// </summary>
		/// <param name="entity">The IEntity instance to register.</param>
		public void AddEntity(IEntity entity)
		{
			entities.UpdateEntity(entity, true);

			if (OnEntityAdded != null)
				OnEntityAdded(entity);
		}

		/// <summary>
		/// Unregisters an IEntity instance from the SystemManager.
		/// </summary>
		/// <param name="entity">The IEntity instance to unregister.</param>
		public void RemoveEntity(IEntity entity)
		{
			entities.UpdateEntity(entity, false);

			if (OnEntityRemoved != null)
				OnEntityRemoved(entity);
		}

		/// <summary>
		/// Unregisters all IEntity instances from the SystemManager.
		/// </summary>
		public void RemoveAllEntities()
		{
			for (int i = 0; i < entities.Count; i++)
			{
				if (OnEntityRemoved != null)
					OnEntityRemoved(entities[i]);
			}

			entities.Clear();
		}

		/// <summary>
		/// Updates an IEntity instance that has had its group or components changed in all IEntityGroup instances.
		/// </summary>
		/// <param name="entity">The modified IEntity instance.</param>
		public void UpdateEntity(IEntity entity)
		{
			entities.UpdateEntity(entity, true);
		}
	}
}
