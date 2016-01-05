using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo.Internal.Entity
{
	public class EntityMatchGroup
	{
		readonly IEntityGroup parent;
		readonly EntityMatches match;
		readonly Dictionary<ByteFlag, EntityGroup> entityGroups = new Dictionary<ByteFlag, EntityGroup>();
		readonly Dictionary<ByteFlag, EntityGroup> componentGroups = new Dictionary<ByteFlag, EntityGroup>();

		public EntityMatchGroup(IEntityGroup parent, EntityMatches match)
		{
			this.parent = parent;
			this.match = match;
		}

		public EntityGroup GetGroupByEntityGroup(ByteFlag groups)
		{
			EntityGroup entityGroup;

			if (!entityGroups.TryGetValue(groups, out entityGroup))
			{
				entityGroup = CreateEntityGroup(groups);
				entityGroups[groups] = entityGroup;
			}

			return entityGroup;
		}

		public EntityGroup GetGroupByComponentGroup(ByteFlag components)
		{
			EntityGroup entityGroup;

			if (!componentGroups.TryGetValue(components, out entityGroup))
			{
				entityGroup = CreateComponentGroup(components);
				componentGroups[components] = entityGroup;
			}

			return entityGroup;
		}

		public void Clear()
		{
			// Entity Groups
			if (entityGroups.Count > 0)
			{
				var enumerator = entityGroups.GetEnumerator();

				while (enumerator.MoveNext())
					enumerator.Current.Value.Clear();

				enumerator.Dispose();
				entityGroups.Clear();
			}

			// Component Groups
			if (componentGroups.Count > 0)
			{
				var enumerator = componentGroups.GetEnumerator();

				while (enumerator.MoveNext())
					enumerator.Current.Value.Clear();

				enumerator.Dispose();
				componentGroups.Clear();
			}
		}

		public void UpdateEntity(IEntity entity, bool isValid)
		{
			// Entity Groups
			if (entityGroups.Count > 0)
			{
				var enumerator = entityGroups.GetEnumerator();

				while (enumerator.MoveNext())
					enumerator.Current.Value.UpdateEntity(entity, isValid && IsEntityGroupValid(entity, enumerator.Current.Key));

				enumerator.Dispose();
			}

			// Component Groups
			if (componentGroups.Count > 0)
			{
				var enumerator = componentGroups.GetEnumerator();

				while (enumerator.MoveNext())
					enumerator.Current.Value.UpdateEntity(entity, isValid && IsComponentGroupValid(entity, enumerator.Current.Key));

				enumerator.Dispose();
			}
		}

		public bool IsEntityGroupValid(IEntity entity, ByteFlag groups)
		{
			return EntityMatch.Matches(entity.Groups.Groups, groups, match);
		}

		public bool IsComponentGroupValid(IEntity entity, ByteFlag components)
		{
			return EntityMatch.Matches(entity, components, match);
		}

		public EntityGroup CreateEntityGroup(ByteFlag groups)
		{
			var entityGroup = new EntityGroup();

			for (int i = 0; i < parent.Entities.Count; i++)
			{
				var entity = parent.Entities[i];
				entityGroup.UpdateEntity(entity, IsEntityGroupValid(entity, groups));
			}

			return entityGroup;
		}

		public EntityGroup CreateComponentGroup(ByteFlag components)
		{
			var entityGroup = new EntityGroup();

			for (int i = 0; i < parent.Entities.Count; i++)
			{
				var entity = parent.Entities[i];
				entityGroup.UpdateEntity(entity, IsComponentGroupValid(entity, components));
			}

			return entityGroup;
		}
	}
}