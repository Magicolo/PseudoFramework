using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pseudo.Internal.Entity3
{
	public interface IEntityGroup
	{
		event Action<IEntity> OnEntityAdded;
		event Action<IEntity> OnEntityRemoved;

		IList<IEntity> Entities { get; }

		IEntityGroup Filter(EntityMatch match);
		IEntityGroup Filter(ByteFlag groups, EntityMatches match = EntityMatches.All);
		IEntityGroup Filter(Type componentType, EntityMatches match = EntityMatches.All);
		IEntityGroup Filter(Type[] componentTypes, EntityMatches match = EntityMatches.All);
	}
}
