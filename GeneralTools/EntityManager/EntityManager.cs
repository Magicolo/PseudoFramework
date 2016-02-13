﻿using Pseudo;
using Pseudo.Internal.Entity;
using Pseudo.Internal.Communication;
using Pseudo.Internal.Pool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.Assertions;
using Zenject;

namespace Pseudo.Internal.Entity
{
	public class EntityManager : IEntityManager
	{
		public IEntityGroup Entities
		{
			get { return entities; }
		}

		readonly EntityGroup entities = new EntityGroup();
		readonly Pool<Entity> entityPool = new Pool<Entity>(new Entity(), () => new Entity(), 0);
		readonly MessageManager messageManager;
		[Inject]
		readonly DiContainer container = null;

		public EntityManager(MessageManager messageManager)
		{
			this.messageManager = messageManager;
		}

		public IEntity CreateEntity()
		{
			return CreateEntity(EntityGroups.Nothing, true);
		}

		public IEntity CreateEntity(EntityGroups groups)
		{
			return CreateEntity(groups, true);
		}

		public IEntity CreateEntity(EntityGroups groups, bool active)
		{
			Assert.IsNotNull(groups);

			var entity = entityPool.Create();
			entity.Initialize(this, messageManager, groups, active);
			AddEntity(entity);

			return entity;
		}

		public IEntity CreateEntity(EntityBehaviour prefab)
		{
			Assert.IsNotNull(prefab);

			var entity = PrefabPoolManager.Create(prefab);
			entity.Initialize(this, container);

			return entity.Entity;
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

		public void AddEntity(IEntity entity)
		{
			UpdateEntity(entity);
		}

		public void RemoveEntity(IEntity entity)
		{
			Assert.IsNotNull(entity);

			entities.UpdateEntity(entity, false);
		}

		public void RemoveAllEntities()
		{
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
