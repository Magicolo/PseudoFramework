using Pseudo;
using Pseudo.EntityFramework.Internal;
using Pseudo.Communication;
using Pseudo.Pooling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.Assertions;

namespace Pseudo.EntityFramework
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
			entity.Initialize(this);

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

		/// <summary>
		/// Adds or updates an IEntity instance to the IEntityGroup hierarchy of the IEntityManager instance.
		/// </summary>
		/// <param name="entity">The IEntity instance to add or update.</param>
		public void AddEntity(IEntity entity)
		{
			Assert.IsNotNull(entity);

			entities.UpdateEntity(entity, true);
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
	}
}
