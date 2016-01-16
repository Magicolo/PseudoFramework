using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pseudo
{
	public interface IEntityGroup
	{
		event Action<IEntity> OnEntityAdded;
		event Action<IEntity> OnEntityRemoved;

		int Count { get; }
		IEntity this[int index] { get; }

		IEntityGroup Filter(EntityMatch match);
		IEntityGroup Filter(EntityGroups groups, EntityMatches match = EntityMatches.All);
		IEntityGroup Filter(Type componentType, EntityMatches match = EntityMatches.All);
		IEntityGroup Filter(Type[] componentTypes, EntityMatches match = EntityMatches.All);
	}
}
