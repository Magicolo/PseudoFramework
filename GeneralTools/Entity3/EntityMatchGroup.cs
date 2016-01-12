using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo.Internal.Entity3
{
	public class EntityMatchGroup
	{
		readonly IEntityGroup parent;
		readonly EntityMatches match;
		readonly Dictionary<ByteFlag, EntityGroup> entityGroups = new Dictionary<ByteFlag, EntityGroup>();
		readonly Dictionary<int[], EntityGroup> componentGroups = new Dictionary<int[], EntityGroup>(ComponentIndicesComparer.Instance);

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

		public EntityGroup GetGroupByComponentIndices(int[] componentIndices)
		{
			EntityGroup entityGroup;

			if (!componentGroups.TryGetValue(componentIndices, out entityGroup))
			{
				entityGroup = CreateComponentGroup(componentIndices);
				componentGroups[componentIndices] = entityGroup;
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
			}

			// Component Groups
			if (componentGroups.Count > 0)
			{
				var enumerator = componentGroups.GetEnumerator();

				while (enumerator.MoveNext())
					enumerator.Current.Value.Clear();

				enumerator.Dispose();
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
			return EntityMatch.Matches(entity.Groups, groups, match);
		}

		public bool IsComponentGroupValid(IEntity entity, int[] componentIndinces)
		{
			return EntityMatch.Matches(entity, componentIndinces, match);
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

		public EntityGroup CreateComponentGroup(int[] componentIndices)
		{
			var entityGroup = new EntityGroup();

			for (int i = 0; i < parent.Entities.Count; i++)
			{
				var entity = parent.Entities[i];
				entityGroup.UpdateEntity(entity, IsComponentGroupValid(entity, componentIndices));
			}

			return entityGroup;
		}
	}
}