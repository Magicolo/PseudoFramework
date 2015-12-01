using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal.Entity;

namespace Pseudo
{
	public class NewEntityGroup
	{
		static EntityGroup.Matches[] matchValues = (EntityGroup.Matches[])Enum.GetValues(typeof(EntityGroup.Matches));

		public event Action<PEntity> OnEntityAdded;
		public event Action<PEntity> OnEntityRemoved;

		readonly EntityGroup.Groups group;
		readonly IEntityGroupMatcher groupMatcher;
		readonly BitArray componentBits;
		readonly IComponentGroupMatcher componentMatcher;
		readonly List<PEntity> entities = new List<PEntity>(8);

		List<NewEntityGroup>[] subGroups;

		public NewEntityGroup(EntityMatch match) : this(match.Group, EntityMatch.GetMatcher(match.Match)) { }

		public NewEntityGroup(EntityGroup.Groups group, EntityGroup.Matches match = EntityGroup.Matches.All) : this(group, EntityMatch.GetMatcher(match)) { }

		public NewEntityGroup(EntityGroup.Groups group, IEntityGroupMatcher matcher)
		{
			this.group = group;
			this.groupMatcher = matcher;
		}

		public NewEntityGroup(BitArray componentBits, EntityGroup.Matches match = EntityGroup.Matches.All) : this(componentBits, ComponentMatch.GetMatcher(match)) { }

		public NewEntityGroup(BitArray componentBits, IComponentGroupMatcher componentMatcher)
		{
			this.componentBits = componentBits;
			this.componentMatcher = componentMatcher;
		}

		public NewEntityGroup Filter(Type[] componentTypes, EntityGroup.Matches match = EntityGroup.Matches.All)
		{
			return GetSubEntityGroup(componentTypes, match);
		}

		public void UpdateEntity(PEntity entity)
		{
			if (groupMatcher != null)
			{
				if (groupMatcher.Matches(entity.Group, group))
					AddEntity(entity);
				else
					RemoveEntity(entity);
			}
			else if (componentMatcher != null)
			{
				if (componentMatcher.Matches(entity, componentBits))
					AddEntity(entity);
				else
					RemoveEntity(entity);
			}
		}

		void AddEntity(PEntity entity)
		{
			if (!entities.Contains(entity))
			{
				entities.Add(entity);
				RaiseOnEntityAdded(entity);
			}
		}

		void RemoveEntity(PEntity entity)
		{
			if (entities.Remove(entity))
				RaiseOnEntityRemoved(entity);
		}

		NewEntityGroup GetSubEntityGroup(Type[] types, EntityGroup.Matches match)
		{
			NewEntityGroup subGroup;

			if (!TryGetSubEntityGroup(types, match, out subGroup))
			{
				subGroup = new NewEntityGroup(EntityUtility.ToComponentBits(types), match);
				subGroups[(int)match].Add(subGroup);
			}

			return subGroup;
		}

		bool TryGetSubEntityGroup(Type[] types, EntityGroup.Matches match, out NewEntityGroup subGroup)
		{
			subGroup = null;

			if (subGroups == null)
				subGroups = new List<NewEntityGroup>[matchValues.Length];

			var groups = subGroups[(int)match];

			if (groups == null)
			{
				subGroups[(int)match] = new List<NewEntityGroup>();
				return false;
			}

			for (int i = 0; i < groups.Count; i++)
			{
				var group = groups[i];
				bool matches = true;

				for (int j = 0; j < types.Length; j++)
				{
					int index = EntityUtility.GetComponentIndex(types[i]);
					matches &= group.componentBits.Count > index && group.componentBits[index];
				}

				if (matches)
				{
					subGroup = group;
					return true;
				}
			}

			return false;
		}

		bool HasComponents(Type[] types)
		{
			for (int i = 0; i < types.Length; i++)
			{
				int index = EntityUtility.GetComponentIndex(types[i]);

				if (componentBits.Count <= index || !componentBits[index])
					return false;
			}

			return true;
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