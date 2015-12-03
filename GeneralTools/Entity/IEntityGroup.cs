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
		IEntityGroup Filter(ByteFlag groups, EntityMatches match = EntityMatches.All);
		IEntityGroup Filter(Type[] types, EntityMatches match = EntityMatches.All);
	}
}