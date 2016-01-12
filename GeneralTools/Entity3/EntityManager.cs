using Pseudo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pseudo.Internal.Entity3
{
	public class EntityManager : IEntityManager
	{
		public static EntityManager Instance = new EntityManager();

		public event Action<IEntity> OnEntityAdded;
		public event Action<IEntity> OnEntityRemoved;
		public IList<IEntity> Entities
		{
			get { return allEntitiesGroup.Entities; }
		}

		readonly EntityGroup allEntitiesGroup = new EntityGroup();

		/// <summary>
		/// Creates a new IEntity instance and adds it to the SystemManager.
		/// </summary>
		/// <returns>The IEntity instance.</returns>
		public IEntity CreateEntity()
		{
			return CreateEntity(ByteFlag.Nothing);
		}

		/// <summary>
		/// Creates a new IEntity instance and adds it to the SystemManager.
		/// </summary>
		/// <param name="groups">The groups that the IEntity instance should be placed in.</param>
		/// <returns>The IEntity instance.</returns>
		public IEntity CreateEntity(ByteFlag groups)
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
			allEntitiesGroup.UpdateEntity(entity, true);

			if (OnEntityAdded != null)
				OnEntityAdded(entity);
		}

		/// <summary>
		/// Unregisters an IEntity instance from the SystemManager.
		/// </summary>
		/// <param name="entity">The IEntity instance to unregister.</param>
		public void RemoveEntity(IEntity entity)
		{
			allEntitiesGroup.UpdateEntity(entity, false);

			if (OnEntityRemoved != null)
				OnEntityRemoved(entity);
		}

		/// <summary>
		/// Unregisters all IEntity instances from the SystemManager.
		/// </summary>
		public void RemoveAllEntities()
		{
			for (int i = 0; i < allEntitiesGroup.Entities.Count; i++)
			{
				if (OnEntityRemoved != null)
					OnEntityRemoved(allEntitiesGroup.Entities[i]);
			}

			allEntitiesGroup.Clear();
		}

		/// <summary>
		/// Updates an IEntity instance that has had its group or components changed in all IEntityGroup instances.
		/// </summary>
		/// <param name="entity">The modified IEntity instance.</param>
		public void UpdateEntity(IEntity entity)
		{
			allEntitiesGroup.UpdateEntity(entity, true);
		}

		/// <summary>
		/// Gets an IEntityGroup instance that contains the IEntity instances that match the provided arguments.
		/// </summary>
		/// <param name="match">The match data to be compared.</param>
		/// <returns>The IEntityGroup instance.</returns>
		public IEntityGroup GetEntityGroup(EntityMatch match)
		{
			return allEntitiesGroup.Filter(match);
		}

		/// <summary>
		/// Gets an IEntityGroup instance that contains the IEntity instances that match the provided arguments.
		/// </summary>
		/// <param name="groups">The groups to be compared.</param>
		/// <param name="match">The match algorithm that should be used.</param>
		/// <returns>The IEntityGroup instance.</returns>
		public IEntityGroup GetEntityGroup(ByteFlag groups, EntityMatches match = EntityMatches.All)
		{
			return allEntitiesGroup.Filter(groups, match);
		}

		/// <summary>
		/// Gets an IEntityGroup instance that contains the IEntity instances that match the provided arguments.
		/// </summary>
		/// <param name="componentType">The component type to be compared.</param>
		/// <param name="match">The match algorithm that should be used.</param>
		/// <returns></returns>
		public IEntityGroup GetEntityGroup(Type componentType, EntityMatches match = EntityMatches.All)
		{
			return allEntitiesGroup.Filter(componentType, match);
		}

		/// <summary>
		/// Gets an IEntityGroup instance that contains the IEntity instances that match the provided arguments.
		/// </summary>
		/// <param name="componentTypes">The component types to be compared.</param>
		/// <param name="match">The match algorithm that should be used.</param>
		/// <returns></returns>
		public IEntityGroup GetEntityGroup(Type[] componentTypes, EntityMatches match = EntityMatches.All)
		{
			return allEntitiesGroup.Filter(componentTypes, match);
		}
	}
}
