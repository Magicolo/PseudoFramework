<<<<<<< HEAD:GeneralTools/Entity/IEntityGroup.cs
﻿using Pseudo.Internal.Entity;
using System;
using System.Collections.Generic;

namespace Pseudo
{
	public interface IEntityGroup
	{
		event Action<PEntity> OnEntityAdded;
		event Action<PEntity> OnEntityRemoved;

		IList<PEntity> Entities { get; }

		void BroadcastMessage(EntityMessages message);
		void BroadcastMessage(EntityMessages message, object argument);
		void BroadcastMessage<T>(EntityMessages message, T argument);
		IEntityGroup Filter(EntityMatch match);
		IEntityGroup Filter(EntityGroups group, EntityMatches match = EntityMatches.All);
		IEntityGroup Filter(ByteFlag<EntityGroups> groups, EntityMatches match = EntityMatches.All);
		IEntityGroup Filter(Type componentType, EntityMatches match = EntityMatches.All);
		IEntityGroup Filter(Type[] componentTypes, EntityMatches match = EntityMatches.All);
	}
=======
﻿using Pseudo.Internal.Entity;
using System;
using System.Collections.Generic;

namespace Pseudo
{
	public interface IEntityGroup
	{
		event Action<IEntity> OnEntityAdded;
		event Action<IEntity> OnEntityRemoved;

		IList<IEntity> Entities { get; }

		IEntityGroup Filter(EntityMatch match);
		IEntityGroup Filter(EntityGroupDefinition groups, EntityMatches match = EntityMatches.All);
		IEntityGroup Filter(Type componentType, EntityMatches match = EntityMatches.All);
		IEntityGroup Filter(Type[] componentTypes, EntityMatches match = EntityMatches.All);
	}
>>>>>>> Entity2:GeneralTools/Entity2/IEntityGroup.cs
}