using Pseudo.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pseudo.EntityFramework
{
	public interface IEntityManager
	{
		IEntityGroup Entities { get; }
		Messager Messager { get; }

		IEntity CreateEntity();
		IEntity CreateEntity(EntityGroups groups);
		IEntity CreateEntity(EntityGroups groups, bool active);
		IEntity CreateEntity(EntityBehaviour prefab);
		void RecycleEntity(IEntity entity);
		void RecycleEntity(EntityBehaviour instance);
		void AddEntity(IEntity entity);
		void RemoveEntity(IEntity entity);
		void RemoveAllEntities();
	}
}
