﻿using System;
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

		void BroadcastMessage(EntityMessage message);
		void BroadcastMessage<TArg>(EntityMessage message, TArg argument);
		void BroadcastMessage<TId>(TId identifier);
		void BroadcastMessage<TId>(TId identifier, MessagePropagation propagation);
		void BroadcastMessage<TId, TArg>(TId identifier, TArg argument);
		void BroadcastMessage<TId, TArg>(TId identifier, TArg argument, MessagePropagation propagation);

		bool Contains(IEntity entity);
		int IndexOf(IEntity entity);
		IEntity Find(Predicate<IEntity> match);
		int FindIndex(Predicate<IEntity> match);
		IEntity[] ToArray();
		void CopyTo(IEntity[] array, int index = 0);
	}
}
