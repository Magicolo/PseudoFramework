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
		readonly Dictionary<ByteFlag, EntityGroup> entityGroups = new Dictionary<ByteFlag, EntityGroup>(2);
		readonly Dictionary<ByteFlag, EntityGroup> componentGroups = new Dictionary<ByteFlag, EntityGroup>(2);

		public EntityMatchGroup(IEntityGroup parent, EntityMatches match)
		{
			this.parent = parent;
			this.match = match;
		}

		public EntityGroup GetEntityGroup(ByteFlag groups)
		{
			EntityGroup entityGroup;

			if (!entityGroups.TryGetValue(groups, out entityGroup))
			{
				entityGroup = CreateEntityGroup(groups);
				entityGroups[groups] = entityGroup;
			}

			return entityGroup;
		}

		public EntityGroup GetEntityGroup(Type[] componentTypes)
		{
			EntityGroup entityGroup;
			var components = EntityUtility.GetComponentFlags(componentTypes);

			if (!componentGroups.TryGetValue(components, out entityGroup))
			{
				entityGroup = CreateEntityComponentGroup(components);
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

		public void UpdateEntity(PEntity entity)
		{
			// Entity Groups
			if (entityGroups.Count > 0)
			{
				var enumerator = entityGroups.GetEnumerator();

				while (enumerator.MoveNext())
				{
					if (IsEntityGroupValid(entity, enumerator.Current.Key))
						enumerator.Current.Value.RegisterEntity(entity);
					else
						enumerator.Current.Value.UnregisterEntity(entity);
				}

				enumerator.Dispose();
			}

			// Component Groups
			if (componentGroups.Count > 0)
			{
				var enumerator = componentGroups.GetEnumerator();

				while (enumerator.MoveNext())
				{
					if (IsEntityComponentsValid(entity, enumerator.Current.Key))
						enumerator.Current.Value.RegisterEntity(entity);
					else
						enumerator.Current.Value.UnregisterEntity(entity);
				}

				enumerator.Dispose();
			}
		}

		public void RegisterEntity(PEntity entity)
		{
			// Entity Groups
			if (entityGroups.Count > 0)
			{
				var enumerator = entityGroups.GetEnumerator();

				while (enumerator.MoveNext())
				{
					if (IsEntityGroupValid(entity, enumerator.Current.Key))
						enumerator.Current.Value.RegisterEntity(entity);
				}

				enumerator.Dispose();
			}

			// Component Groups
			if (componentGroups.Count > 0)
			{
				var enumerator = componentGroups.GetEnumerator();

				while (enumerator.MoveNext())
				{
					if (IsEntityComponentsValid(entity, enumerator.Current.Key))
						enumerator.Current.Value.RegisterEntity(entity);
				}

				enumerator.Dispose();
			}
		}

		public void UnregisterEntity(PEntity entity)
		{
			// Entity Groups
			if (entityGroups.Count > 0)
			{
				var enumerator = entityGroups.GetEnumerator();

				while (enumerator.MoveNext())
					enumerator.Current.Value.UnregisterEntity(entity);

				enumerator.Dispose();
			}

			// Component Groups
			if (componentGroups.Count > 0)
			{
				var enumerator = componentGroups.GetEnumerator();

				while (enumerator.MoveNext())
					enumerator.Current.Value.UnregisterEntity(entity);

				enumerator.Dispose();
			}
		}

		public bool IsEntityGroupValid(PEntity entity, ByteFlag groups)
		{
			return EntityMatch.Matches(entity.Group, groups, match);
		}

		public bool IsEntityComponentsValid(PEntity entity, ByteFlag components)
		{
			return EntityMatch.Matches(entity, components, match);
		}

		public EntityGroup CreateEntityGroup(ByteFlag groups)
		{
			var entityGroup = new EntityGroup();

			for (int i = 0; i < parent.Entities.Count; i++)
			{
				var entity = parent.Entities[i];

				if (IsEntityGroupValid(entity, groups))
					entityGroup.RegisterEntity(entity);
			}

			return entityGroup;
		}

		public EntityGroup CreateEntityComponentGroup(ByteFlag components)
		{
			var entityGroup = new EntityGroup();

			for (int i = 0; i < parent.Entities.Count; i++)
			{
				var entity = parent.Entities[i];

				if (IsEntityComponentsValid(entity, components))
					entityGroup.RegisterEntity(entity);
			}

			return entityGroup;
		}
	}
}