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
		readonly static EntityMatches[] matchValues = (EntityMatches[])Enum.GetValues(typeof(EntityMatches));

		public event Action<IEntity> OnEntityAdded;
		public event Action<IEntity> OnEntityRemoved;
		public int Count
		{
			get { return entities.Count; }
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

		public bool Contains(IEntity entity)
		{
			return hashedEntities.Contains(entity);
		}

		public void Clear()
		{
			OnEntityAdded = null;
			OnEntityRemoved = null;
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
			if (hashedEntities.Add(entity))
			{
				entities.Add(entity);

				if (OnEntityAdded != null)
					OnEntityAdded(entity);
			}
		}

		void UnregisterEntity(IEntity entity)
		{
			if (hashedEntities.Remove(entity))
			{
				entities.Remove(entity);

				if (OnEntityRemoved != null)
					OnEntityRemoved(entity);
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