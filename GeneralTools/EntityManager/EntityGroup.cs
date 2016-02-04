using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo.Internal.Entity
{
	public class EntityGroup : IEntityGroup
	{
		static readonly EntityMatches[] matchValues = (EntityMatches[])Enum.GetValues(typeof(EntityMatches));
		static readonly IMessageManager messageManager = new MessageManager();

		public event Action<IEntity> OnEntityAdded = delegate { };
		public event Action<IEntity> OnEntityRemoved = delegate { };
		public int Count
		{
			get { return hashedEntities.Count; }
		}
		public IEntity this[int index]
		{
			get { return entities[index]; }
		}

		readonly HashSet<IEntity> hashedEntities = new HashSet<IEntity>();
		readonly List<IEntity> entities = new List<IEntity>();
		readonly EntityMatchGroup[] subGroups = new EntityMatchGroup[matchValues.Length];

		public IEntityGroup Filter(EntityMatch match)
		{
			return Filter(match.Groups, match.Match);
		}

		public IEntityGroup Filter(EntityGroups groups, EntityMatches match = EntityMatches.All)
		{
			return GetMatchGroup(match).GetGroupByEntityGroup(groups);
		}

		public IEntityGroup Filter(Type componentType, EntityMatches match = EntityMatches.All)
		{
			return GetMatchGroup(match).GetGroupByComponentIndices(ComponentUtility.GetComponentIndices(componentType));
		}

		public IEntityGroup Filter(Type[] componentTypes, EntityMatches match = EntityMatches.All)
		{
			return GetMatchGroup(match).GetGroupByComponentIndices(ComponentUtility.GetComponentIndices(componentTypes));
		}

		public void BroadcastMessage<TId>(TId identifier)
		{
			for (int i = entities.Count - 1; i >= 0; i--)
				messageManager.Send(entities[i], identifier);
		}

		public void BroadcastMessage<TId, TArg>(TId identifier, TArg argument)
		{
			for (int i = entities.Count - 1; i >= 0; i--)
				messageManager.Send(entities[i], identifier, argument);
		}

		public void BroadcastMessage<TId, TArg1, TArg2>(TId identifier, TArg1 argument1, TArg2 argument2)
		{
			for (int i = entities.Count - 1; i >= 0; i--)
				messageManager.Send(entities[i], identifier, argument1, argument2);
		}

		public void BroadcastMessage<TId, TArg1, TArg2, TArg3>(TId identifier, TArg1 argument1, TArg2 argument2, TArg3 argument3)
		{
			for (int i = entities.Count - 1; i >= 0; i--)
				messageManager.Send(entities[i], identifier, argument1, argument2, argument3);
		}

		public bool Contains(IEntity entity)
		{
			return hashedEntities.Contains(entity);
		}

		public int IndexOf(IEntity entity)
		{
			return entities.IndexOf(entity);
		}

		public IEntity Find(Predicate<IEntity> match)
		{
			return entities.Find(match);
		}

		public int FindIndex(Predicate<IEntity> match)
		{
			return entities.FindIndex(match);
		}

		public IEntity[] ToArray()
		{
			return entities.ToArray();
		}

		public void CopyTo(IEntity[] array, int index = 0)
		{
			entities.CopyTo(array, index);
		}

		public void Clear()
		{
			OnEntityAdded = delegate { };
			OnEntityRemoved = delegate { };
			hashedEntities.Clear();
			entities.Clear();

			for (int i = 0; i < subGroups.Length; i++)
			{
				var subGroup = subGroups[i];

				if (subGroup != null)
					subGroup.Clear();
			}
		}

		public void UpdateEntity(IEntity entity, bool isValid)
		{
			if (isValid)
				RegisterEntity(entity);
			else
				UnregisterEntity(entity);

			for (int i = 0; i < subGroups.Length; i++)
			{
				var subGroup = subGroups[i];

				if (subGroup != null)
					subGroup.UpdateEntity(entity, isValid);
			}
		}

		void RegisterEntity(IEntity entity)
		{
			if (!Contains(entity))
			{
				hashedEntities.Add(entity);
				entities.Add(entity);
				OnEntityAdded(entity);
			}
		}

		void UnregisterEntity(IEntity entity)
		{
			if (Contains(entity))
			{
				OnEntityRemoved(entity);
				hashedEntities.Remove(entity);
				entities.Remove(entity);
			}
		}

		EntityMatchGroup GetMatchGroup(EntityMatches match)
		{
			var matchGroup = subGroups[(int)match];

			if (matchGroup == null)
			{
				matchGroup = new EntityMatchGroup(this, match);
				subGroups[(int)match] = matchGroup;
			}

			return matchGroup;
		}
	}
}