using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pseudo.Internal.Entity3
{
	public interface IEntityManager
	{
		event Action<IEntity> OnEntityAdded;
		event Action<IEntity> OnEntityRemoved;

		IList<IEntity> Entities { get; }

		IEntity CreateEntity();
		IEntity CreateEntity(ByteFlag groups);
		void AddEntity(IEntity entity);
		void RemoveEntity(IEntity entity);
		void RemoveAllEntities();
		void UpdateEntity(IEntity entity);

		IEntityGroup GetEntityGroup(EntityMatch match);
		IEntityGroup GetEntityGroup(ByteFlag groups, EntityMatches match = EntityMatches.All);
		IEntityGroup GetEntityGroup(Type componentType, EntityMatches match = EntityMatches.All);
		IEntityGroup GetEntityGroup(Type[] componentTypes, EntityMatches match = EntityMatches.All);
	}
}
