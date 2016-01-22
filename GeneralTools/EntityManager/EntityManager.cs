﻿using Pseudo;
using Pseudo.Internal.Entity;
using Pseudo.Internal.Pool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.Assertions;

namespace Pseudo
{
	public class EntityManager : IEntityManager
	{
		public event Action<IEntity> OnEntityAdded = delegate { };
		public event Action<IEntity> OnEntityRemoved = delegate { };
		public IEntityGroup Entities
		{
			get { return entities; }
		}

		readonly EntityGroup entities = new EntityGroup();
		readonly Pool<Entity> entityPool = new Pool<Entity>(new Entity(), () => new Entity(), 16);

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
			Assert.IsNotNull(groups);

			var entity = entityPool.Create();
			entity.Initialize(this, groups);
			AddEntity(entity);

			return entity;
		}

		public EntityBehaviour CreateEntity(EntityBehaviour prefab)
		{
			Assert.IsNotNull(prefab);

			var entity = PrefabPoolManager.Create(prefab);
			entity.Initialize(this);

			return entity;
		}

		public void RecycleEntity(IEntity entity)
		{
			Assert.IsNotNull(entity);

			RemoveEntity(entity);
			entityPool.Recycle(entity);
		}

		public void RecycleEntity(EntityBehaviour instance)
		{
			Assert.IsNotNull(instance);

			PrefabPoolManager.Recycle(instance);
		}

		/// <summary>
		/// Registers an IEntity instance to the SystemManager.
		/// </summary>
		/// <param name="entity">The IEntity instance to register.</param>
		public void AddEntity(IEntity entity)
		{
			Assert.IsNotNull(entity);

			entities.UpdateEntity(entity, true);
			OnEntityAdded(entity);
		}

		/// <summary>
		/// Unregisters an IEntity instance from the SystemManager.
		/// </summary>
		/// <param name="entity">The IEntity instance to unregister.</param>
		public void RemoveEntity(IEntity entity)
		{
			Assert.IsNotNull(entity);

			entities.UpdateEntity(entity, false);
			OnEntityRemoved(entity);
		}

		/// <summary>
		/// Unregisters all IEntity instances from the SystemManager.
		/// </summary>
		public void RemoveAllEntities()
		{
			for (int i = 0; i < entities.Count; i++)
				OnEntityRemoved(entities[i]);

			entities.Clear();
		}

		/// <summary>
		/// Updates an IEntity instance that has had its group or components changed in all IEntityGroup instances.
		/// </summary>
		/// <param name="entity">The modified IEntity instance.</param>
		public void UpdateEntity(IEntity entity)
		{
			Assert.IsNotNull(entity);

			entities.UpdateEntity(entity, true);
		}
	}
}