using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal.EntityOld;

namespace Pseudo.Internal.EntityOld
{
	public class EntityGroup : IEntityGroupOld
	{
		static EntityMatchesOld[] matchValues = (EntityMatchesOld[])Enum.GetValues(typeof(EntityMatchesOld));

		public event Action<IEntityOld> OnEntityAdded;
		public event Action<IEntityOld> OnEntityRemoved;
		public IList<IEntityOld> Entities
		{
			get { return entities; }
		}

		readonly List<IEntityOld> entities = new List<IEntityOld>(2);
		readonly EntityMatchGroup[] subGroups = new EntityMatchGroup[matchValues.Length];

		public IEntityGroupOld Filter(ByteFlag groups, EntityMatchesOld match = EntityMatchesOld.All)
		{
			return GetMatchGroup(match).GetGroupByEntityGroup(groups);
		}

		public IEntityGroupOld Filter(EntityMatchOld match)
		{
			return Filter(match.Groups, match.Match);
		}

		public IEntityGroupOld Filter(Type componentType, EntityMatchesOld match = EntityMatchesOld.All)
		{
			return GetMatchGroup(match).GetGroupByComponentGroup(EntityUtility.GetComponentFlags(componentType));
		}

		public IEntityGroupOld Filter(Type[] componentTypes, EntityMatchesOld match = EntityMatchesOld.All)
		{
			return GetMatchGroup(match).GetGroupByComponentGroup(EntityUtility.GetComponentFlags(componentTypes));
		}

		public void BroadcastMessage(EntityMessages message)
		{
			for (int i = 0; i < entities.Count; i++)
				entities[i].SendMessage(message);
		}

		public void BroadcastMessage<T>(EntityMessages message, T argument)
		{
			for (int i = 0; i < entities.Count; i++)
				entities[i].SendMessage(message, argument);
		}

		public void BroadcastMessage(EntityMessages message, object argument)
		{
			for (int i = 0; i < entities.Count; i++)
				entities[i].SendMessage(message, argument);
		}

		public void Clear()
		{
			entities.Clear();

			for (int i = 0; i < subGroups.Length; i++)
			{
				var subGroup = subGroups[i];

				if (subGroup != null)
					subGroup.Clear();
			}

			//subGroups.Clear();
		}

		public void UpdateEntity(IEntityOld entity, bool isValid)
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

		void RegisterEntity(IEntityOld entity)
		{
			if (!entities.Contains(entity))
			{
				entities.Add(entity);
				RaiseOnEntityAdded(entity);
			}
		}

		void UnregisterEntity(IEntityOld entity)
		{
			if (entities.Remove(entity))
				RaiseOnEntityRemoved(entity);
		}

		EntityMatchGroup GetMatchGroup(EntityMatchesOld match)
		{
			var matchGroup = subGroups[(int)match];

			if (matchGroup == null)
			{
				matchGroup = new EntityMatchGroup(this, match);
				subGroups[(int)match] = matchGroup;
			}

			return matchGroup;
		}

		protected virtual void RaiseOnEntityAdded(IEntityOld entity)
		{
			if (OnEntityAdded != null)
				OnEntityAdded(entity);
		}

		protected virtual void RaiseOnEntityRemoved(IEntityOld entity)
		{
			if (OnEntityRemoved != null)
				OnEntityRemoved(entity);
		}
	}
}