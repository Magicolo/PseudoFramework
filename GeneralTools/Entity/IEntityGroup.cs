using Pseudo.Internal.Entity;
using System;
using System.Collections.Generic;

namespace Pseudo
{
	public interface IEntityGroup
	{
		event Action<PEntity> OnEntityAdded;
		event Action<PEntity> OnEntityRemoved;

		IList<PEntity> Entities { get; }

		IEntityGroup Filter(EntityMatch match);
		IEntityGroup Filter(EntityGroups group, EntityMatches match = EntityMatches.All);
		IEntityGroup Filter(ByteFlag<EntityGroups> groups, EntityMatches match = EntityMatches.All);
		IEntityGroup Filter(Type componentType, EntityMatches match = EntityMatches.All);
		IEntityGroup Filter(Type[] componentTypes, EntityMatches match = EntityMatches.All);
	}
}