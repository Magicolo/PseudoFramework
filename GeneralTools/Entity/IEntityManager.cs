using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pseudo
{
	public interface IEntityManager
	{
		event Action<IEntity> OnEntityAdded;
		event Action<IEntity> OnEntityRemoved;
		IEntityGroup Entities { get; }

		IEntity CreateEntity();
		IEntity CreateEntity(EntityGroups groups);
		EntityBehaviour CreateEntity(EntityBehaviour prefab);
		void RecycleEntity(IEntity entity);
		void RecycleEntity(EntityBehaviour instance);
		void AddEntity(IEntity entity);
		void RemoveEntity(IEntity entity);
		void RemoveAllEntities();
		void UpdateEntity(IEntity entity);
	}
}
