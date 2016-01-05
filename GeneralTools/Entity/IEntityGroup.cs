<<<<<<< HEAD
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
		IEntityGroup Filter(ByteFlag groups, EntityMatches match = EntityMatches.All);
		IEntityGroup Filter(Type componentType, EntityMatches match = EntityMatches.All);
		IEntityGroup Filter(Type[] componentTypes, EntityMatches match = EntityMatches.All);
	}
>>>>>>> e6176370f888e6e8807b0b4438b063cfc4318d34
}