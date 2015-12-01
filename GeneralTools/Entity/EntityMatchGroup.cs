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
		readonly IEntityGroupMatcher matcher;
		readonly Dictionary<ulong, EntityGroup> entityGroups = new Dictionary<ulong, EntityGroup>(16);

		public EntityMatchGroup(EntityGroup.Matches match) : this(EntityMatch.GetMatcher(match)) { }

		public EntityMatchGroup(IEntityGroupMatcher matcher)
		{
			this.matcher = matcher;
		}

		public EntityGroup GetEntityGroup(EntityGroup.Groups group)
		{
			EntityGroup entityGroup;

			if (!entityGroups.TryGetValue((ulong)group, out entityGroup))
			{
				entityGroup = EntityUtility.CreateEntityGroup(group, matcher);
				entityGroups[(ulong)group] = entityGroup;
			}

			return entityGroup;
		}

		public void Clear()
		{
			var enumerator = entityGroups.GetEnumerator();

			while (enumerator.MoveNext())
				enumerator.Current.Value.Clear();

			enumerator.Dispose();
			entityGroups.Clear();
		}

		public void UpdateEntity(PEntity entity)
		{
			var enumerator = entityGroups.GetEnumerator();

			while (enumerator.MoveNext())
			{
				var entityGroup = enumerator.Current;

				if (matcher.Matches(entity.Group, (EntityGroup.Groups)entityGroup.Key))
					entityGroup.Value.Add(entity);
				else
					entityGroup.Value.Remove(entity);
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
				group.Value.Remove(entity);
			}
		}
	}
}