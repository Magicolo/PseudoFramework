using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo.Internal.EntityOld
{
	public class EntityMatchGroup
	{
		readonly IEntityGroupOld parent;
		readonly EntityMatchesOld match;
		readonly Dictionary<ByteFlag, EntityGroupOld> entityGroups = new Dictionary<ByteFlag, EntityGroupOld>();
		readonly Dictionary<ByteFlag, EntityGroupOld> componentGroups = new Dictionary<ByteFlag, EntityGroupOld>();

		public EntityMatchGroup(IEntityGroupOld parent, EntityMatchesOld match)
		{
			this.parent = parent;
			this.match = match;
		}

		public EntityGroupOld GetGroupByEntityGroup(ByteFlag groups)
		{
			EntityGroupOld entityGroup;

			if (!entityGroups.TryGetValue(groups, out entityGroup))
			{
				entityGroup = CreateEntityGroup(groups);
				entityGroups[groups] = entityGroup;
			}

			return entityGroup;
		}

		public EntityGroupOld GetGroupByComponentGroup(ByteFlag components)
		{
			EntityGroupOld entityGroup;

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
				//entityGroups.Clear();
			}

			// Component Groups
			if (componentGroups.Count > 0)
			{
				var enumerator = componentGroups.GetEnumerator();

				while (enumerator.MoveNext())
					enumerator.Current.Value.Clear();

				enumerator.Dispose();
				//componentGroups.Clear();
			}
		}

		public void UpdateEntity(IEntityOld entity, bool isValid)
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

		public bool IsEntityGroupValid(IEntityOld entity, ByteFlag groups)
		{
			return EntityMatchOld.Matches(entity.Groups, groups, match);
		}

		public bool IsComponentGroupValid(IEntityOld entity, ByteFlag components)
		{
			return EntityMatchOld.Matches(entity, components, match);
		}

		public EntityGroupOld CreateEntityGroup(ByteFlag groups)
		{
			var entityGroup = new EntityGroupOld();

			for (int i = 0; i < parent.Entities.Count; i++)
			{
				var entity = parent.Entities[i];
				entityGroup.UpdateEntity(entity, IsEntityGroupValid(entity, groups));
			}

			return entityGroup;
		}

		public EntityGroupOld CreateComponentGroup(ByteFlag components)
		{
			var entityGroup = new EntityGroupOld();

			for (int i = 0; i < parent.Entities.Count; i++)
			{
				var entity = parent.Entities[i];
				entityGroup.UpdateEntity(entity, IsComponentGroupValid(entity, components));
			}

			return entityGroup;
		}
	}
}