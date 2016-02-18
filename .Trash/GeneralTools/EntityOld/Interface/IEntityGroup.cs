using Pseudo.Internal.EntityOld;
using System;
using System.Collections.Generic;

namespace Pseudo.Internal.EntityOld
{
	public interface IEntityGroupOld
	{
		event Action<IEntityOld> OnEntityAdded;
		event Action<IEntityOld> OnEntityRemoved;

		IList<IEntityOld> Entities { get; }

		IEntityGroupOld Filter(EntityMatchOld match);
		IEntityGroupOld Filter(ByteFlag groups, EntityMatchesOld match = EntityMatchesOld.All);
		IEntityGroupOld Filter(Type componentType, EntityMatchesOld match = EntityMatchesOld.All);
		IEntityGroupOld Filter(Type[] componentTypes, EntityMatchesOld match = EntityMatchesOld.All);
		void BroadcastMessage(EntityMessages message);
		void BroadcastMessage<T>(EntityMessages message, T argument);
		void BroadcastMessage(EntityMessages message, object argument);
	}
}