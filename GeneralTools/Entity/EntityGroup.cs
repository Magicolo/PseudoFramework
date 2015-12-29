﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal.Entity;

namespace Pseudo.Internal.Entity
{
	public class EntityGroup : IEntityGroup
	{
		static EntityMatches[] matchValues = (EntityMatches[])Enum.GetValues(typeof(EntityMatches));

		public event Action<PEntity> OnEntityAdded;
		public event Action<PEntity> OnEntityRemoved;
		public IList<PEntity> Entities
		{
			get { return readonlyEntities; }
		}

		readonly List<PEntity> entities = new List<PEntity>(2);
		readonly IList<PEntity> readonlyEntities;
		EntityMatchGroup[] subGroups;

		public EntityGroup()
		{
			readonlyEntities = entities.AsReadOnly();
		}

		public void BroadcastMessage(EntityMessages message)
		{
			for (int i = 0; i < entities.Count; i++)
				entities[i].SendMessage(message);
		}

		public void BroadcastMessage(EntityMessages message, object argument)
		{
			for (int i = 0; i < entities.Count; i++)
				entities[i].SendMessage(message, argument);
		}

		public void BroadcastMessage<T>(EntityMessages message, T argument)
		{
			for (int i = 0; i < entities.Count; i++)
				entities[i].SendMessage(message, argument);
		}

		public IEntityGroup Filter(EntityGroups group, EntityMatches match = EntityMatches.All)
		{
			var flag = new ByteFlag();
			flag[(byte)group] = true;

			return Filter(flag, match);
		}

		public IEntityGroup Filter(ByteFlag<EntityGroups> groups, EntityMatches match = EntityMatches.All)
		{
			return GetMatchGroup(match).GetEntityGroupByGroup(groups);
		}

		public IEntityGroup Filter(EntityMatch match)
		{
			return Filter(match.Groups, match.Match);
		}

		public IEntityGroup Filter(Type componentType, EntityMatches match = EntityMatches.All)
		{
			return GetMatchGroup(match).GetEntityGroupByComponent(EntityUtility.GetComponentFlags(componentType));
		}

		public IEntityGroup Filter(Type[] componentTypes, EntityMatches match = EntityMatches.All)
		{
			return GetMatchGroup(match).GetEntityGroupByComponent(EntityUtility.GetComponentFlags(componentTypes));
		}

		public void Clear()
		{
			entities.Clear();

			if (subGroups == null)
				return;

			for (int i = 0; i < subGroups.Length; i++)
			{
				var subGroup = subGroups[i];

				if (subGroup != null)
					subGroup.Clear();
			}

			subGroups.Clear();
		}

		public void UpdateEntity(PEntity entity)
		{
			if (subGroups != null)
			{
				for (int i = 0; i < subGroups.Length; i++)
				{
					var subGroup = subGroups[i];

					if (subGroup != null)
						subGroup.UpdateEntity(entity);
				}
			}
		}

		public void RegisterEntity(PEntity entity)
		{
			if (!entities.Contains(entity))
			{
				entities.Add(entity);
				RaiseOnEntityAdded(entity);

				if (subGroups != null)
				{
					for (int i = 0; i < subGroups.Length; i++)
					{
						var subGroup = subGroups[i];

						if (subGroup != null)
							subGroup.RegisterEntity(entity);
					}
				}
			}
		}

		public void UnregisterEntity(PEntity entity)
		{
			if (entities.Remove(entity))
			{
				RaiseOnEntityRemoved(entity);

				if (subGroups != null)
				{
					for (int i = 0; i < subGroups.Length; i++)
					{
						var subGroup = subGroups[i];

						if (subGroup != null)
							subGroup.UnregisterEntity(entity);
					}
				}
			}
		}

		EntityMatchGroup GetMatchGroup(EntityMatches match)
		{
			if (subGroups == null)
				subGroups = new EntityMatchGroup[matchValues.Length];

			var matchGroup = subGroups[(int)match];

			if (matchGroup == null)
			{
				matchGroup = new EntityMatchGroup(this, match);
				subGroups[(int)match] = matchGroup;
			}

			return matchGroup;
		}

		protected virtual void RaiseOnEntityAdded(PEntity entity)
		{
			if (OnEntityAdded != null)
				OnEntityAdded(entity);
		}

		protected virtual void RaiseOnEntityRemoved(PEntity entity)
		{
			if (OnEntityRemoved != null)
				OnEntityRemoved(entity);
		}
	}
}