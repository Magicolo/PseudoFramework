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
		static readonly EntityAllGroupMatcher allMatcher = new EntityAllGroupMatcher();
		static readonly EntityAnyGroupMatcher anyMatcher = new EntityAnyGroupMatcher();
		static readonly EntityNoneGroupMatcher noneMatcher = new EntityNoneGroupMatcher();
		static readonly EntityExactGroupMatcher exactMatcher = new EntityExactGroupMatcher();

		readonly IEntityGroupMatcher matcher;
		readonly Dictionary<ulong, EntityGroup> entityGroups = new Dictionary<ulong, EntityGroup>(16);

		public static IEntityGroupMatcher GetMatcher(EntityMatch.Matches match)
		{
			IEntityGroupMatcher matcher = null;

			switch (match)
			{
				case EntityMatch.Matches.All:
					matcher = allMatcher;
					break;
				case EntityMatch.Matches.Any:
					matcher = anyMatcher;
					break;
				case EntityMatch.Matches.None:
					matcher = noneMatcher;
					break;
				case EntityMatch.Matches.Exact:
					matcher = exactMatcher;
					break;
			}

			return matcher;
		}

		public EntityMatchGroup(EntityMatch.Matches match) : this(GetMatcher(match)) { }

		public EntityMatchGroup(IEntityGroupMatcher matcher)
		{
			this.matcher = matcher;
		}

		public EntityGroup GetEntityGroup(EntityMatch.Groups group)
		{
			EntityGroup entityGroup;

			if (!entityGroups.TryGetValue((ulong)group, out entityGroup))
			{
				entityGroup = CreateEntityGroup(group);
				entityGroups[(ulong)group] = entityGroup;
			}

			return entityGroup;
		}

		public void UpdateEntity(PEntity entity)
		{
			var enumerator = entityGroups.GetEnumerator();

			while (enumerator.MoveNext())
			{
				var entityGroup = enumerator.Current;

				if (matcher.Matches(entity.Group, (EntityMatch.Groups)entityGroup.Key))
					entityGroup.Value.AddEntity(entity);
				else
					entityGroup.Value.RemoveEntity(entity);
			}

			enumerator.Dispose();
		}

		public void RegisterEntity(PEntity entity)
		{
			UpdateEntity(entity);
		}

		public void UnregisterEntity(PEntity entity)
		{
			var enumerator = entityGroups.GetEnumerator();
			while (enumerator.MoveNext())
			{
				var group = enumerator.Current;
				group.Value.RemoveEntity(entity);
			}
		}

		EntityGroup CreateEntityGroup(EntityMatch.Groups group)
		{
			var entityGroup = new EntityGroup();
			var allEntities = EntityManager.GetAllEntities();

			for (int i = 0; i < allEntities.Count; i++)
			{
				var entity = allEntities[i];

				if (matcher.Matches(entity.Group, group))
					entityGroup.AddEntity(entity);
			}

			return entityGroup;
		}
	}
}